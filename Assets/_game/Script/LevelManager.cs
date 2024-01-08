using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject brick;
    [SerializeField] GameObject bridge;
    [SerializeField] Transform[] bridges;
    [SerializeField] Transform[] brickContainer ;
    [SerializeField] Transform[] Origins;
    int takenBrickFloor1 = 0; 
    int takenBrickFloor2 = 0; 
    List<int> takenColor = new List<int>();
    Transform bridgeContainer;

    public static LevelManager instance;
    
    private void Start()
    {
        instance = this;
        for(int i=0; i<Origins.Length; i++)
        {
            PlaceBrick(Origins[i].position, brickContainer[i]);
        }
        for(int i=0; i<bridges.Length; i++)
        {
            SpawnBridge(bridges[i].position);
        }
    }
    private void Update()
    {
        RespawnTakenBrick();
    }
    //spawn the brick on the floor
    private void PlaceBrick(Vector3 origin,Transform Parent)
    {
        int countTheBrickNumber = 0;
        Vector3 newpos;
        for (int i = 0; i < 10; i++)
        {
            newpos = origin;
            newpos.z += (float)3.5 * i;
            for(int j = 0; j < 10; j++)
            {
                int queque=GetColor(countTheBrickNumber);
                newpos.x += (float)4.15;
                GameObject clone = Instantiate(brick,newpos, Quaternion.identity,Parent);
                clone.GetComponent<Brick>().SetColor(queque);
                countTheBrickNumber++;
                if(origin.y <15)
                    Floor1BrickPool.instance.AddToThePool(clone);
                else
                    Floor2BrickPool.instance.AddToThePool(clone);
            }
        }
    }
    private int GetColor(int count)
    {
        int[] remainingBricks = new int[4] { 25, 25, 25, 25 };

        // Create an array to store the shuffled brick colors
        int[] shuffledBricks = new int[100];
        for (int i = 0; i < shuffledBricks.Length; i++)
        {
            // Calculate the total number of remaining bricks
            int totalRemainingBricks = 0;
            for (int j = 0; j < remainingBricks.Length; j++)
            {
                totalRemainingBricks += remainingBricks[j];
            }
            int randomNumber = Random.Range(0, totalRemainingBricks);

            int brickColor = -1;
            for (int j = 0; j < remainingBricks.Length; j++)
            {
                if (randomNumber < remainingBricks[j])
                {
                    brickColor = j;
                    remainingBricks[j]--;
                    break;
                }
                else
                {
                    randomNumber -= remainingBricks[j];
                }
            }
            shuffledBricks[i] = brickColor;
        }
        return shuffledBricks[count];
    }
    //Spawn the brick of the bridge
    private void SpawnBridge(Vector3 pos)
    {
        bridgeContainer = new GameObject().transform;
        for (int i = 0; i < 20; i++)
        {   
            Instantiate(bridge, pos, Quaternion.identity,bridgeContainer);
            pos.y += 1;
            pos.z +=1.75f;
        }
    }
    //add the taken brick position to the list and count the color
    public void AddToList(int color,int floor)
    {
        takenColor.Add(color);
        switch (floor)
        {
            case 0:
                takenBrickFloor1++;
                break;
            case 1:
                takenBrickFloor2++;
                break;
        }
    }
    //respawn the taken brick
    void RespawnTakenBrick()
    {
        if(takenBrickFloor1 >= 15)
        {
            int rand = Random.Range(0, takenColor.Count);
            int color = takenColor[rand];            
            GameObject Clone = Floor1BrickPool.instance.GetPooledBrick();
            Clone.SetActive(true);
            Clone.GetComponent<Brick>().SetColor(color);
            takenColor.RemoveAt(rand);
            takenBrickFloor1--;
        }
        if (takenBrickFloor2 >= 15)
        {
            int rand = Random.Range(0, takenColor.Count);
            int color = takenColor[rand];
            GameObject Clone = Floor2BrickPool.instance.GetPooledBrick();
            Clone.SetActive(true);
            Clone.GetComponent<Brick>().SetColor(color);
            takenColor.RemoveAt(rand);
            takenBrickFloor2--;
        }
    }
}

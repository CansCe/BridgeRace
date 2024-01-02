using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] brickList;
    [SerializeField] GameObject bridge;
    [SerializeField] Transform bridge1,bridge2;
    
    Vector3 floor1_origin = new Vector3(3, 0, 5);
    Vector3 floor2_origin = new Vector3(3, 20, 87);
    int red = 25;
    int blue = 25;
    int green = 25;
    int yellow = 25;
    private List<Vector3> takenBrickList = new List<Vector3>();

    public Transform brickContainer;
    public Transform bridgeContainer;

    public static GameManager instance;
    
    private void Start()
    {
        instance = this;
        PlaceBrick(floor1_origin);
        PlaceBrick(floor2_origin);
        SpawnBridge(bridge1.position);
        SpawnBridge(bridge2.position);
    }
    private void Update()
    {
        RespawnTakenBrick();
    }
    //spawn the brick on the floor
    private void PlaceBrick(Vector3 origin)
    {
        int countTheBrickNumber = 0;
        //spawn brick per floor 
        //4 bricks 25 each
        brickContainer = new GameObject().transform;
        Vector3 newpos = origin;
        for (int i = 0; i < 10; i++)
        {
            newpos = origin;
            newpos.z += (float)3.5 * i;
            for(int j = 0; j < 10; j++)
            {
                int queque=GetColor(countTheBrickNumber);
                newpos.x += (float)4.15;
                Instantiate(brickList[queque],newpos, Quaternion.identity,brickContainer);
                countTheBrickNumber++;
            }
        }
    }
    //get the color of the brick | red = 0 | blue = 1 | green = 2 | yellow = 3
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
    public void AddToList(Vector3 pos,int color)
    {
       takenBrickList.Add(pos);
       switch (color)
        {
            case 0:
                red++;
                break;
            case 1:
                blue++;
                break;
            case 2:
                green++;
                break;
            case 3:
                yellow++;
                break;
        }
    }
    //respawn the taken brick
    void RespawnTakenBrick()
    {
        if(takenBrickList.Count > 5)
        {
            for(int i = 0; i < takenBrickList.Count; i++)
            {
                int color = Random.Range(0,3);
                Instantiate(brickList[color], takenBrickList[i], Quaternion.identity, brickContainer);
                //remove the spawned brick from the list
                takenBrickList.RemoveAt(i);
            }
            return;
        }
    }
}

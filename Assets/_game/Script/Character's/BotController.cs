using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Purchasing;

public class BotController : MonoBehaviour
{
    [SerializeField] GameObject backBrickContainer;
    [SerializeField] GameObject brick;
    [SerializeField] Transform destination;
    [SerializeField] Transform[] BrickContainer;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Rigidbody rb;

    int numberBrickCollected = 0;
    int color = 0;
    
    public int currentFloor =0;

    void Start()
    {

        StartCollectBrick();
    }

    // Update is called once per frame
    void Update()
    {
        //shoot a raycast to check what floor is the bot standing on
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100))
        {
            if (hit.collider.gameObject.CompareTag("Floor1"))
            {
                currentFloor = 0;
            }
            else if(hit.collider.gameObject.CompareTag("Floor2"))
            {
                currentFloor = 1;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Brick")
        {
            if (color == other.GetComponent<Brick>().color)
            {
                Stop();
                Vector3 pos = new Vector3(backBrickContainer.transform.position.x, backBrickContainer.transform.position.y + (float)(1.25 * numberBrickCollected), backBrickContainer.transform.position.z);
                numberBrickCollected += 1;
                Gamemanager.instance.AddToList(other.gameObject.transform.position, other.GetComponent<Brick>().color);
                other.transform.SetParent(null);
                Destroy(other.gameObject);
                Instantiate(brick, pos, transform.rotation, backBrickContainer.transform);
                Debug.Log(numberBrickCollected);
                if(numberBrickCollected <= 10)
                {
                    StartCollectBrick();
                }
                else
                {
                    agent.destination = destination.position;
                }
            }
        }else
        //if player hit the bridge brick
        if (other.CompareTag("BridgeBrick"))
        {
            if (numberBrickCollected > 0)
            {
                if (other.gameObject.GetComponent<BridgeBrick>().canBePlace == false
                    && color == other.gameObject.GetComponent<BridgeBrick>().color)
                {
                    return;
                }
                else
                {
                    numberBrickCollected--;
                    Debug.Log("Remaining " + numberBrickCollected + color);
                    other.gameObject.GetComponent<BridgeBrick>().placed(color);
                    Destroy(backBrickContainer.transform.GetChild(numberBrickCollected).gameObject);
                }
            }
            else if (numberBrickCollected <= 0)
            {
                if(other.gameObject.GetComponent<BridgeBrick>().canBePlace)
                {
                    Debug.Log("STOP");
                    Stop();
                    agent.velocity = Vector3.zero;
                }
                StartCollectBrick();
            }
        }
    }
    //wait till the bot reach the destination
    void StartCollectBrick()
    {
        if (BrickContainer[currentFloor] == null)
        {
            return;
        }
        Vector3 nextPos = GetClosestBrick(BrickContainer[currentFloor]).position;
        agent.destination = nextPos;
    }
    Transform GetClosestBrick(Transform Container)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        for (int i=0; i< Container.childCount; i++)
        {
            Transform t = Container.GetChild(i);
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                //if the child has the same color
                if (t.GetComponent<Brick>().color == color)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
        }
        return tMin;
    }
    void Stop()
    {
        agent.isStopped = true;
        //decrease the velocity of the agent
        //minus half of the velocity of the agent
        agent.velocity = agent.velocity - agent.velocity / (float)(1.5);
        agent.isStopped = false;
    }
    //call if player hit the bot
    public void Hit()
    {
        //loose all brick collected
        numberBrickCollected = 0;
        //loose all brick in the back
        for (int i = 0; i < backBrickContainer.transform.childCount; i++)
        {
            Destroy(backBrickContainer.transform.GetChild(i).gameObject);
        }
        //unfreeze all rotation and then fall down
        rb.constraints = RigidbodyConstraints.None;
    }
}
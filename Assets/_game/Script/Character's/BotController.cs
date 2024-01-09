using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Purchasing;

public class BotController : Character
{
    [SerializeField] GameObject backBrickContainer;
    [SerializeField] GameObject brick;
    [SerializeField] Transform destination;
    [SerializeField] Transform finishLine;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Rigidbody rb;
    

    int numberBrickCollected = 0;
    int targetNumBrick;
    public int color;
    public Transform[] BrickContainer = new Transform[2];
    public int currentFloor = 0;

    //velocity and path of the agent before pausing
    Vector3 lastAgentDes;

    protected override void OnDestroy()
    {
        StateManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }
    void Start()
    {
        targetNumBrick = Random.Range(10, 20);
        StartCollectBrick();
        StateManager.instance.OnGameStateChanged += OnGameStateChanged;
    }

    void pause()
    {
        lastAgentDes = agent.destination;
        PauseAnim();
        agent.velocity = Vector3.zero;
        agent.transform.position = transform.position;
        agent.ResetPath();
    }

    void resume()
    {
        //resume all 
        agent.destination = GetClosestBrick(BrickContainer[currentFloor]).position;
        ResumeAnim();
    }

    void Update()
    {
        if (agent.velocity != Vector3.zero)
        {
            ChangeAnim("run");
        }
        if (agent.velocity == Vector3.zero && agent.destination == null)
            ChangeAnim("idle");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100))
        {
            if (hit.collider.gameObject.CompareTag("Floor1"))
            {
                currentFloor = 0;
            }
            else if (hit.collider.gameObject.CompareTag("Floor2"))
            {
                currentFloor = 1;
            }
            else if (hit.collider.gameObject.CompareTag("Floor2a"))
            {
                currentFloor = 1;
            }
            else if (hit.collider.gameObject.CompareTag("Floor2b"))
            {
                currentFloor = 2;
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
                LevelManager.instance.AddToList(color, currentFloor);
                other.gameObject.SetActive(false);
                Instantiate(brick, pos, transform.rotation, backBrickContainer.transform);
                if (numberBrickCollected <= targetNumBrick)
                {
                    StartCollectBrick();
                }
                else
                {
                    targetNumBrick = Random.Range(10, 20);
                    agent.destination = destination.position;
                }
            }
        }
        else
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
                    other.gameObject.GetComponent<BridgeBrick>().placed(color);
                    Destroy(backBrickContainer.transform.GetChild(numberBrickCollected).gameObject);
                }
            }
            else if (numberBrickCollected <= 0)
            {
                if (color == other.gameObject.GetComponent<BridgeBrick>().color)
                    return;
                if (other.gameObject.GetComponent<BridgeBrick>().canBePlace)
                {
                    agent.velocity = Vector3.zero;
                    Stop();
                }
                StartCollectBrick();
            }
        }else 
        if (other.gameObject.CompareTag("Finish"))
        {
            Stop();
            Debug.Log("reach the destination");
            agent.SetDestination(finishLine.position);
            ChangeAnim("victory");
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
        for (int i = 0; i < Container.childCount; i++)
        {
            Transform t = Container.GetChild(i);
            if (t.gameObject.activeInHierarchy == false)
            {
                continue;
            }
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                //if the child has the same color
                if (t.GetComponent<Brick>().color == color )
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
        agent.ResetPath();
        agent.velocity = agent.velocity - agent.velocity / (float)(1.5);
        agent.isStopped = false;
    }
    public void Hit()
    {
        numberBrickCollected = 0;
        for (int i = 0; i < backBrickContainer.transform.childCount; i++)
        {
            Destroy(backBrickContainer.transform.GetChild(i).gameObject);
        }
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    protected override void OnGameStateChanged(IState newGameState)
    {
        Debug.Log("Player unable");
        if(newGameState == IState.Start)
        {
            resume();
        }
        else
        {
            pause();
        }
    }
}
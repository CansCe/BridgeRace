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
    [SerializeField] Transform Destination;
    [SerializeField] Transform[] BrickContainer;

    
    int numberBrickCollected = 0;
    int color = 0;
    int currentFloor;

    void Start()
    {
        GetComponent<NavMeshAgent>().destination = Destination.position;
    }

    // Update is called once per frame
    void Update()
    {
        //shoot a raycast to check what floor is the bot standing on
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2))
        {
            if (hit.collider.gameObject.CompareTag("Floor1"))
            {
                currentFloor = 1;
            }
            else if (hit.collider.gameObject.CompareTag("Floor2"))
            {
                currentFloor = 2;
            }
        }
    }

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Brick")
        {
            if (color == other.GetComponent<Brick>().color)
            {
                Vector3 pos = new Vector3(backBrickContainer.transform.position.x, backBrickContainer.transform.position.y + (float)(1.25 * numberBrickCollected), backBrickContainer.transform.position.z);
                numberBrickCollected += 1;
                Gamemanager.instance.AddToList(other.gameObject.transform.position, other.GetComponent<Brick>().color);
                Destroy(other.gameObject);
                Instantiate(brick, pos, transform.rotation, backBrickContainer.transform);
                Debug.Log(numberBrickCollected);
            }
        }else
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
                    Debug.Log("Remaining " + numberBrickCollected);
                    other.gameObject.GetComponent<BridgeBrick>().placed(color);
                    Destroy(backBrickContainer.transform.GetChild(numberBrickCollected).gameObject);
                }
            }
            else if (numberBrickCollected <= 0)
            {
                
                GetComponent<NavMeshAgent>().SetDestination(transform.position);
                GetComponent<NavMeshAgent>().isStopped = true;
                StartCollectingBrickState();
            }
        }
    }
    private void StartCollectingBrickState()
    {
        if (currentFloor == 1)
        {
            for (int i = 0; i < 22; i++)
            {
                Transform nextPos = GetClosestBrick(BrickContainer[0]);
                GetComponent<NavMeshAgent>().SetDestination(nextPos.position);
                StartCoroutine(Wait());
                StopAllCoroutines();
            }
            GetComponent<NavMeshAgent>().destination = Destination.position;
        }
        else if (currentFloor == 2)
        {
            for (int i = 0; i < 22; i++)
            {
                Transform nextPos = GetClosestBrick(BrickContainer[1]);
                GetComponent<NavMeshAgent>().destination = nextPos.position;
                Wait();
            }
            GetComponent<NavMeshAgent>().destination = Destination.position;
        }
    }
    //wait till the bot reach the destination
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
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
}

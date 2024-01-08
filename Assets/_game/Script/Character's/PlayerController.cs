using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] Rigidbody rb;
    [SerializeField] FixedJoystick joystick;
    [SerializeField] GameObject backBrickContainer;
    [SerializeField] GameObject brick;
    [SerializeField] Transform finishLine;

    bool isWin = false;
    float speed = 15;
    int color = 1;
    int numberBrickCollected = 0;
    Vector3 nexPosition;

    void Start()
    {
        
    }

    void Update()
    {
        nexPosition = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        if (nexPosition != Vector3.zero)
        {
            if(!isWin)
                ChangeAnim("run");
            transform.rotation = Quaternion.LookRotation(nexPosition);
        }
        else
        {
            ChangeAnim("idle");
        }

    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100))
        {
            if (hit.collider.gameObject.CompareTag("Bridge"))
            {
                if (nexPosition.z < 0)
                {
                    nexPosition.y -=1.25f;
                }
            }
        }
        rb.MovePosition(transform.position + nexPosition * speed * Time.deltaTime);
    }

    //trigger event 
    private void OnTriggerEnter(Collider other)
    {
        //if player hit the wall
        if (other.gameObject.CompareTag("Wall"))
        {
            //can't phase through wall
            transform.position = transform.position - nexPosition * speed * Time.deltaTime;
        }
        //if player hit the brick
        if (other.gameObject.tag == "Brick")
        {
            if (color == other.gameObject.GetComponent<Brick>().color)
            {
                Vector3 pos = new Vector3(backBrickContainer.transform.position.x, backBrickContainer.transform.position.y + (float)(1.25 * numberBrickCollected), backBrickContainer.transform.position.z);
                numberBrickCollected += 1;
                LevelManager.instance.AddToList(other.gameObject.transform.position,other.GetComponent<Brick>().color);
                Destroy(other.gameObject);
                Instantiate(brick,pos, transform.rotation, backBrickContainer.transform);
            }
        }
        //if player hit the bridge brick
        if (other.gameObject.CompareTag("BridgeBrick"))
        {
            if (numberBrickCollected > 0 )
            {
                if(other.gameObject.GetComponent<BridgeBrick>().canBePlace == true || other.gameObject.GetComponent<BridgeBrick>().color != color)
                {
                    PlaceBrickOnTheBridge();
                    other.GetComponent<BridgeBrick>().placed(color);
                }
            }
            else if (numberBrickCollected<=0)
            {
                if(other.gameObject.GetComponent<BridgeBrick>().canBePlace == false)
                {
                    if(other.gameObject.GetComponent<BridgeBrick>().color != color)
                    {
                        ConstrainPlayer();
                    }
                }
                else
                {
                    ConstrainPlayer();
                }
            }
        }else
        if (other.gameObject.tag == "finish")
        {
            isWin = true;
            Debug.Log("Reach Destination");
            joystick.enabled = false;
            transform.position = Vector3.MoveTowards(transform.position, finishLine.position, 10 * Time.deltaTime);
            ChangeAnim("win");
        }
        //if player hit the bot
        if(other.gameObject.tag=="Bot"){
            other.gameObject.GetComponent<BotController>().Hit();
        }
    }

    void PlaceBrickOnTheBridge()
    {
        transform.position = transform.position + new Vector3(0, 0.25f, 0);
        numberBrickCollected--;
        Destroy(backBrickContainer.transform.GetChild(numberBrickCollected).gameObject);
    }
    void ConstrainPlayer()
    {
        if (nexPosition.z > 0)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
        }
        else if (nexPosition.z < 0)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
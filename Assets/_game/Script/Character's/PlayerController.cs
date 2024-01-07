using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] Rigidbody rb;
    [SerializeField] FixedJoystick joystick;
    [SerializeField] GameObject backBrickContainer;
    [SerializeField] GameObject brick;
    [SerializeField] Transform finishLine;
    

    float speed = 15;
    int color = 1;
    int numberBrickCollected = 0;
    Vector3 nexPosition;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 newpos = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        nexPosition = newpos;
        if (newpos != Vector3.zero)
        {
            ChangeAnim("run");
            transform.rotation = Quaternion.LookRotation(newpos);
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
                Gamemanager.instance.AddToList(other.gameObject.transform.position,other.GetComponent<Brick>().color);
                Destroy(other.gameObject);
                Instantiate(brick,pos, transform.rotation, backBrickContainer.transform);
            }
        }
        //if player hit the bridge brick
        if (other.gameObject.CompareTag("BridgeBrick"))
        {
            if (numberBrickCollected > 0 )
            {
                if(other.gameObject.GetComponent<BridgeBrick>().canBePlace == false 
                    && color == other.gameObject.GetComponent<BridgeBrick>().color)
                {
                    return;
                }
                else
                {
                    //make the player go up a bit higher
                    transform.position = transform.position + new Vector3(0, 0.25f, 0);
                    numberBrickCollected--;
                    other.gameObject.GetComponent<BridgeBrick>().placed(color);
                    Destroy(backBrickContainer.transform.GetChild(numberBrickCollected).gameObject);
                }
            }
            else if (numberBrickCollected<=0)
            {
                if(other.gameObject.GetComponent<BridgeBrick>().canBePlace == false)
                {
                    return;
                }
                else if(color != other.gameObject.GetComponent<BridgeBrick>().color)
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
                else
                {
                    return;
                }
                
            }
        }else
        if (other.gameObject.tag == "finish")
        {
            //player can not move via joystick
            joystick.gameObject.SetActive(false);
            //move player toward the finish line
            transform.position = Vector3.MoveTowards(transform.position, finishLine.position, 10 * Time.deltaTime);
            ChangeAnim("victory");
        }
        //if player hit the bot
        if(other.gameObject.tag=="Bot"){
            other.gameObject.GetComponent<BotController>().Hit();
        }
    }
}
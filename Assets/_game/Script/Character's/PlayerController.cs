using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] FixedJoystick joystick;
    [SerializeField] GameObject backBrickContainer;
    [SerializeField] GameObject brick;

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

        //rotate the player to the direction of moving
        if (newpos != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(newpos);
        }
    }

    //moving is in fixedupdate to get sync with the follwoing camera
    private void FixedUpdate()
    {
        //move player using rigidbody
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
            rb.velocity = Vector3.zero;
        }else 
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
                Debug.Log(numberBrickCollected);
            }
        }else 
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
                    numberBrickCollected--;
                    Debug.Log("Remaining " + numberBrickCollected);
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
                else if (nexPosition.z>0) 
                {
                    //make it static 
                    rb.constraints = RigidbodyConstraints.FreezePositionZ;
                }
                else if (nexPosition.z < 0)
                {
                    //undo the static 
                    rb.constraints = RigidbodyConstraints.None;
                    //still freeze the rotation x y z
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                }
            }
        }else
        //if player hit the bot
        if(other.gameObject.tag=="Bot"){
            other.gameObject.GetComponent<BotController>().Hit();
        }
    }
}
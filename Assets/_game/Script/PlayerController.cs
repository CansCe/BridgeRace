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

    // Update is called once per frame
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
        rb.MovePosition(rb.position + nexPosition * speed * Time.fixedDeltaTime);
    }
    //trigger event 
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Brick")
        {
            if (color == other.GetComponent<Brick>().color)
            {
                Vector3 pos = new Vector3(backBrickContainer.transform.position.x, backBrickContainer.transform.position.y + (float)(1.25 * numberBrickCollected), backBrickContainer.transform.position.z);
                numberBrickCollected += 1;
                GameManager.instance.AddToList(other.gameObject.transform.position,other.GetComponent<Brick>().color);
                Destroy(other.gameObject);
                Instantiate(brick,pos, transform.rotation, backBrickContainer.transform);
                Debug.Log(numberBrickCollected);
            }
        }
        if (other.gameObject.CompareTag("BridgeBrick"))
        {
            if (numberBrickCollected > 0 )
            {
                if(other.gameObject.GetComponent<BridgeBrick>().canBePlace == false)
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
                if (nexPosition.z>0) 
                {
                    //make it static 
                    rb.constraints = RigidbodyConstraints.FreezePositionZ;
                }
                else if (nexPosition.z < 0)
                {
                    //undo the static 
                    rb.constraints = RigidbodyConstraints.None;
                }
            }
        }
    }
}
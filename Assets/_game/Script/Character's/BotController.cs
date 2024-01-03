using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Purchasing;

public class BotController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _speed = 3f;
    [SerializeField] GameObject backBrickContainer;
    [SerializeField] GameObject brick;

    int numberBrickCollected = 0;
    int color = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        }
        if (other.CompareTag("BridgeBrick"))
        {
            

        }
    }
}

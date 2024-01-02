using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBrick : MonoBehaviour
{
    [SerializeField] Material[] mats;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //active mesh renderer
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            int color = other.gameObject.GetComponent<PlayerController>().getOwnColor();
            //aplly the color to the brick 
            gameObject.GetComponent<MeshRenderer>().material = mats[color];
        }
    }
}

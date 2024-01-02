using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBrick : MonoBehaviour
{
    [SerializeField] Material[] mats;
    int color;
    public bool canBePlace = true;
    
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //active mesh renderer
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            int playerColor= other.gameObject.GetComponent<PlayerController>().getOwnColor();
            if(playerColor != color)
            {
                color = playerColor;
                gameObject.GetComponent<MeshRenderer>().material = mats[color];
            }
            else
            {
                return;
            }
        }
    }

    public void placed()
    {
        canBePlace = !canBePlace;
    }
}

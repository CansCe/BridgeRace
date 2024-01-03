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
    
    public void placed(int playerColor)
    {
        
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        if (playerColor != color)
        {
            color = playerColor;
            gameObject.GetComponent<MeshRenderer>().material = mats[color];
            canBePlace = !canBePlace;
        }
        else
        {
            return;
        }
    }
}

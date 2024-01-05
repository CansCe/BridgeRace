using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBrick : MonoBehaviour
{
    [SerializeField] Material[] mats;
    public int color = 5;
    public bool canBePlace = true;
    
    public void placed(int playerColor)
    {   
        //if the mesh renderer is disabled, enable it (this is for the first brick)
        if (!gameObject.GetComponent<MeshRenderer>().enabled)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            Debug.Log("Repaint");
            color = playerColor;
            gameObject.GetComponent<MeshRenderer>().material = mats[color];
            canBePlace = !canBePlace;
        }
        if (playerColor != color)
        {
            color = playerColor;
            gameObject.GetComponent<MeshRenderer>().material = mats[color];
            canBePlace = !canBePlace;
        }
        else if (canBePlace == true && color != playerColor)
        {
            Debug.Log("Repaint");
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

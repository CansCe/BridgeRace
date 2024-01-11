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
            color = playerColor;
            gameObject.GetComponent<MeshRenderer>().material = mats[color];
            canBePlace = !canBePlace;
        }
        if (playerColor != color)
        {
            if (canBePlace == true)
            {
                canBePlace = !canBePlace;
            }
            color = playerColor;
            gameObject.GetComponent<MeshRenderer>().material = mats[color];
        }
        else
        {
            return;
        }
    }

    public void Reset()
    {
        GetComponent<MeshRenderer>().enabled = false;
        Debug.Log(Equals(gameObject.GetComponent<MeshRenderer>().enabled));
        canBePlace = true;
        color = 5;
    }
}

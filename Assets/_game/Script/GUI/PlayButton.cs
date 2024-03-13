using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    [SerializeField] Canvas canvas;
   //invole the game manager to start the game
   public void PlayGame()
   {
        Debug.Log("Clicked");
        canvas.enabled = false;
    }
}

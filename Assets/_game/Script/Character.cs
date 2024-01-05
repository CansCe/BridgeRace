using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    iSate currentState;

    void Start()
    {
        currentState = new IdleState();
    }

    // Update is called once per frame
    void ChangeState(iSate newState)
    {
       if(currentState != newState)
       {
            currentState = newState;
       }
    }
}

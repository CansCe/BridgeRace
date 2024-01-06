using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState :iSate
{
    public void OnEnter()
    {
        Debug.Log("IdleState OnEnter");
    }

    public void OnExecute()
    {
        Debug.Log("IdleState OnExecute");
    }

    public void OnExit()
    {
        Debug.Log("IdleState OnExit");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Character _instance;

    private void Awake()
    {
        _instance = GetComponent<Character>();
        StateManager.instance.OnGameStateChanged += OnGameStateChanged;
    }

    protected virtual void OnDestroy()
    {
        StateManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }

    public Animator anim;
    private string currentAnim;
    public void ChangeAnim(string animName)
    {
        if(currentAnim != animName)
        {
            if (currentAnim != null)
                anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    protected void PauseAnim()
    {
        anim.speed = 0;
    }
    protected void ResumeAnim()
    {
        anim.speed = 1;
    }

    protected virtual void OnGameStateChanged(IState newGameState)
    {
        Debug.Log("Player unable");
        enabled = newGameState == IState.Start;
        if(!enabled)
        {
            PauseAnim();
        }
        else
        {
            ResumeAnim();
        }
    }
}

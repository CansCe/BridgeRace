using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Character _instance;
    public Animator anim;
    private string currentAnim;

    private void Awake()
    {
        _instance = GetComponent<Character>();
        StateManager.instance.OnGameStateChanged += OnGameStateChanged;
    }

    protected virtual void OnDestroy()
    {
        StateManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
        DontDestroyOnLoad(anim);
    }
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
        if (anim == null)
        {
            return;
        }
        anim.speed = 0;
    }
    protected void ResumeAnim()
    {
        if (anim == null)
        {
            return;
        }
        anim.speed = 1;
    }

    protected virtual void OnGameStateChanged(IState newGameState)
    {
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

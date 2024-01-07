using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    [System.Obsolete]
    public void OnClick()
    {
        StateManager.instance.SetState(IState.Start);
        GameManager.instance.Reload();
        //change game state to play
    }
}

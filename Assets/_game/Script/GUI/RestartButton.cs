using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    [SerializeField] GameObject pauseCanvas;
    [System.Obsolete]
    public void OnClick()
    {
        StateManager.instance.SetState(IState.Start);
        LevelManager.instance.ResetLevel();
    }
}

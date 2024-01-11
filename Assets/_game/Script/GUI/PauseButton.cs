using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseButton : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    [SerializeField] GameObject pausedCanvas;
    [SerializeField] GameObject pause;

    [System.Obsolete]
    public void OnClick()
    {
        IState currentState = StateManager.instance.CurrentGameState;
        IState newGameState = currentState == IState.Start ? IState.Pause : IState.Start;
        StateManager.instance.SetState(newGameState);
        if (pause != null)
            pause.active = !pause.activeInHierarchy;
        if (newGameState == IState.Pause)
        {
            joystick.enabled = false;
            pausedCanvas.active = true;
        }
        else
        {
            joystick.enabled = true;
            pausedCanvas.active = false;
        }
    }
}

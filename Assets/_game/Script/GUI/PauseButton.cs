using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] Joystick joystick;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        joystick.enabled = true;
        IState currentState = StateManager.instance.CurrentGameState;
        IState newGameState = currentState == IState.Start ? IState.Pause : IState.Start;
        Debug.Log("Player pressed pause");
        StateManager.instance.SetState(newGameState);
    }
}

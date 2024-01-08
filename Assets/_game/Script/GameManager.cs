using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //invole the game manager to start the game
    
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Update()
    {
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene(sceneName: "Level1");
    }
}

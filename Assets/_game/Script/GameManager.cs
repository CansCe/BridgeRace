using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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

    [Obsolete]
    public void StartGame()
    {
        LevelManager.instance.OnInit();
    }
    public void StartLevel2()
    {
        SceneManager.LoadScene(sceneName: "Level2");
    }

    [System.Obsolete]
    public void Reload()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene != null)
        {
            SceneManager.LoadScene("TempLoadScene");
            SceneManager.LoadSceneAsync(scene.name);
        }
    }

    internal void LoadBackToMenu()
    {
        
    }
}

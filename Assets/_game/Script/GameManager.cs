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
    public void StartLevel1()
    {
        SceneManager.LoadScene(sceneName: "Level1");
    }
    public void StartLevel2()
    {
        SceneManager.LoadScene(sceneName: "Level2");
    }

    [System.Obsolete]
    public void Reload()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.UnloadScene(scene.name);
        SceneManager.LoadScene(scene.name);
    }
}

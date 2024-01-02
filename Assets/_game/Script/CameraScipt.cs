using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScipt : MonoBehaviour
{
    [SerializeField] Vector3 addingVector;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position+ addingVector;
    }

    private void Awake()
    { 
        player = GameObject.FindGameObjectWithTag("Player");
    }
}

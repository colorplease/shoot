using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera[] players;
    [SerializeField]int cameraIndex;

    public Transform[] spawns;
    void Awake()
    {
        GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Cam");
        players = new Camera[gameObjectArray.Length];
        for(int i = 0; i < gameObjectArray.Length; i++)
        {
            players[i] = gameObjectArray[i].GetComponent<Camera>();
            
        }

        GameObject[] spawnGameObjectArray = GameObject.FindGameObjectsWithTag("Spawn");
        spawns = new Transform[spawnGameObjectArray.Length];
        for(int i = 0; i < spawnGameObjectArray.Length; i++)
        {
            spawns[i] = spawnGameObjectArray[i].GetComponent<Transform>();
            
        }

        players[0] = GameObject.Find("Main Camera").GetComponent<Camera>();
        
    }
    void Update()
    {
        GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Cam");
        players = new Camera[gameObjectArray.Length];
        for(int i = 0; i < gameObjectArray.Length; i++)
        {
            players[i] = gameObjectArray[i].GetComponent<Camera>();
            
        }
        CameraSwitching();
    }

void CameraSwitching()
{
    if (Input.GetKeyDown(KeyCode.Q))
        {
            if (cameraIndex > 0)
            {
                cameraIndex--; 
                players[cameraIndex + 1].targetDisplay = 1;
                players[cameraIndex + 1].gameObject.GetComponent<AudioListener>().enabled = false; 
                players[cameraIndex].targetDisplay = 0;    
                players[cameraIndex].gameObject.GetComponent<AudioListener>().enabled = true;            
            }
            else
            {
                cameraIndex = players.Length - 1;
                players[0].targetDisplay = 1;
                players[0].gameObject.GetComponent<AudioListener>().enabled = false; 
                players[cameraIndex].targetDisplay = 0;    
                players[cameraIndex].gameObject.GetComponent<AudioListener>().enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (cameraIndex < players.Length - 1)
            {
                cameraIndex++;
                players[cameraIndex - 1].targetDisplay = 1;
                players[cameraIndex - 1].gameObject.GetComponent<AudioListener>().enabled = false; 
                players[cameraIndex].targetDisplay = 0;    
                players[cameraIndex].gameObject.GetComponent<AudioListener>().enabled = true; 
            }
            else
            {
                cameraIndex = 0;
                players[players.Length - 1].targetDisplay = 1;
                players[players.Length - 1].gameObject.GetComponent<AudioListener>().enabled = false; 
                players[cameraIndex].targetDisplay = 0;    
                players[cameraIndex].gameObject.GetComponent<AudioListener>().enabled = true;
            }
        }
    if (cameraIndex > 0)
    {
    
    }
    else if (cameraIndex == 0)
    {
    }
}
}

using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System.IO;
using UnityEngine.Profiling;
using UnityEngine.Timeline;

public class ManualReplay : MonoBehaviour
{
    private MemoryStream memoryStream = null;
    private BinaryWriter binaryWriter = null;
    private BinaryReader binaryReader = null;

    private List<Vector3> Actors;
    public Transform target;
    private int currentkey;
    
    private void Awake()
    {
        //Actors = new Dictionary<string, Transform>();
        Actors = new List<Vector3>();
        currentkey = 0;
            
        //no sé si ésto va aquí...
        memoryStream = new MemoryStream();
        binaryWriter = new BinaryWriter(memoryStream);
        binaryReader = new BinaryReader(memoryStream);
    }
    
    private void InitRecording()
    {
        memoryStream.SetLength(0);
        memoryStream.Seek(0, SeekOrigin.Begin);
        binaryWriter.Seek(0, SeekOrigin.Begin);
            
        print("Recoding started at: " + Time.time);
    }

    public void StoreKey(Transform transf)
    {
        Actors.Add(new Vector3(transf.position.x, transf.position.y, transf.position.z)); 
        print("Key recorded at: " + Time.time);
    }

    public void LoadKey(Transform transf)
    {
        //target.position = Actors
        currentkey = Mathf.Clamp(currentkey, 0, Actors.Count - 1);
        
        transf.position = Actors[currentkey];
        currentkey++;
    }


    void Start()
    {
        InitRecording();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) //o for Recording a key
        {
            StoreKey(target);
        }

        if (Input.GetKeyDown(KeyCode.P)) // p for Replaying a key
        {
            LoadKey(target);
        }
        
        //movimiento normal del juego
        target.Translate(Input.GetAxis("Horizontal") * Time.deltaTime,Input.GetAxis("Vertical") * Time.deltaTime,0f);
        
    }
}

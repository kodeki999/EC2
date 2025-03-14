using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class feedField1 : MonoBehaviour
{

    MeshFilter plane;
    objectsFields _feed;
    objectsFields _trash;


    //Variabili da settare
    public GameObject area;
    public GameObject feed;
    public GameObject trash;
    public Camera cam;
    public int objects_in_scene;
    public int time_max;
    public int time_slot;
    public int decrement_factor;
    public int increment_factor;


    public static feedField1 Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        plane = GetComponent<MeshFilter>();

        _trash = new objectsFields();
        _trash.ObjectsFields(plane, area, trash, cam, objects_in_scene, time_slot, decrement_factor);
        _trash.InitField();

        _feed = new objectsFields();
        _feed.ObjectsFields(plane, area, feed, cam, objects_in_scene, time_slot, decrement_factor);
        _feed.InitField();
        _feed.RandInstantiateObject();
        
        InvokeRepeating("Spam", 0f, time_slot);
    }
    private void Spam()
    {
        _feed.DecreasingObjects();
        _trash.Spawning(increment_factor);
        
    }

    public void DestroyObject(GameObject _destroy)
    {
        _feed.DestoyObjects(_destroy);
    }

    void Update()
    {
    }
}
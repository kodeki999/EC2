using System;
using UnityEngine;

public class Sea_moving : MonoBehaviour
{
    [Header("Altezza che si vuole raggiungere (es. 2.8)")]
    public float height = 2.8f;



    [HideInInspector]
    public float time_max;
    [HideInInspector]
    public Vector3 pos;

    [HideInInspector]
    public float secCounter = 0;

    void Start()
    {
        pos = transform.position;
      
        time_max = feedField.Instance.time_max;
    }

    // Update is called once per frame
    void Update()
    {
        secCounter += Time.deltaTime;

        float t = secCounter / time_max;
        Vector3 step = new Vector3(pos.x, Mathf.Lerp(pos.y, height, t), pos.z);

        
        transform.position = step;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class compress_env : MonoBehaviour
{

    [Header("Altezza che si vuole raggiungere (es. 2.8)")]
    public Vector3 vet = new Vector3(0,0,0);

    public Vector3 angle = new Vector3(0,0,0);

    [HideInInspector]
    public float time_max;
    [HideInInspector]
    public Vector3 pos;

    [HideInInspector]
    public float secCounter = 0;
    [HideInInspector]
    public Quaternion rot;
    [HideInInspector]
    public Quaternion distRot;


    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        time_max = feedField.Instance.time_max;
        rot = transform.rotation;
        distRot = Quaternion.Euler(angle);
    }
    
    void Update()
    {
        secCounter += Time.deltaTime;

        float t = secCounter / time_max;
        Vector3 step = Vector3.Lerp(pos, vet, t);
        Quaternion end = Quaternion.Lerp(rot, distRot, t);

        transform.rotation = end;
        transform.position = step;
    }

}

using System.Collections.Generic;
using UnityEngine;

public class objectsFields
{


    //Variabili private

    private Vector3[] planes_points;
    private Vector3[] areas_points = new Vector3[4];
    private Vector3[] fields_points;
    private Dictionary<int, bool> tmp_dict = new Dictionary<int, bool>();
    private List<GameObject> instantiated_objects = new List<GameObject>();
    private int[] lut;
    private int num_fields_points = 0; /// <summary>
                                       /// variabile che riporta il numero di punti nel campo. Sovrascritta nella funzione GetNumFieldPoints()
                                       /// Differenza con object_to_instantiate: uno serve per instanziare gli oggetti, l'altra per inizializzare un array
                                       /// i due valori non coincidono per forza. num_fields_points > bjects_to_instantiate deve essere vero per un buon funzionamento del codice
                                       /// </summary>
    private int current_object_number;



    //Variabili di istanza
    private MeshFilter plane;
    private GameObject area;
    private GameObject _object;
    private Camera cam;
    private int objects_to_instantiate;
    private int time_slot;
    private int decrement_factor;







    // Void di configurazione
    public void ObjectsFields(MeshFilter plane, GameObject area, GameObject _object, Camera cam, int objects_to_instatiate,  int time_slot, int decrement_factor)
    {
        this.plane = plane;
        this.area = area;
        this._object = _object;
        this.cam = cam;
        this.objects_to_instantiate = objects_to_instatiate;
        this.time_slot = time_slot;
        this.decrement_factor = decrement_factor;
    }




    //overloading
    public void ObjectsFields(MeshFilter plane, GameObject area)
    {
        this.plane = plane;
        this.area = area;
    }
    // Funzioni interfaccia

    public void debug()
    {
        Debug.Log("Classe objectsFields funzionante");
    }

    public void InitField()
    {
        LutInit();
        GetAreaVertices(area);
        GetPlaneVertices(plane);
        if (areas_points == null)
        {
            Debug.LogError("no hai accesso all'area del quadrato");
        }
        Vector3 min = areas_points[0];
        Vector3 max = areas_points[0];
        foreach (Vector3 v in areas_points)
        {
            min = Vector3.Min(min, v);
            max = Vector3.Max(max, v);
        }
        for (int i = 0; i < planes_points.Length; i++)
        {
            bool isInside = planes_points[i].x >= min.x && planes_points[i].x <= max.x &&
                            planes_points[i].z >= min.z && planes_points[i].z <= max.z;

            tmp_dict.Add(i, isInside);
        }
        GetNumFieldPoints();
        fields_points = new Vector3[num_fields_points];
        GetFieldPoint(planes_points, fields_points, tmp_dict);
    }
    public void InitField(string over)
    {
        if (over == "particles")
        {
            GetAreaVertices(area);
            GetPlaneVertices(plane);
            if (areas_points == null)
            {
                Debug.LogError("no hai accesso all'area del quadrato");
            }
            Vector3 min = areas_points[0];
            Vector3 max = areas_points[0];
            foreach (Vector3 v in areas_points)
            {
                min = Vector3.Min(min, v);
                max = Vector3.Max(max, v);
            }
            for (int i = 0; i < planes_points.Length; i++)
            {
                bool isInside = planes_points[i].x >= min.x && planes_points[i].x <= max.x &&
                                planes_points[i].z >= min.z && planes_points[i].z <= max.z;

                tmp_dict.Add(i, isInside);
            }
            GetNumFieldPoints();
            fields_points = new Vector3[num_fields_points];
            GetFieldPoint(planes_points, fields_points, tmp_dict);
        }
        else
        {
            Debug.Log("set argoument = particles ");
        }
    }

    public void RandInstantiateObject()
    {

        for (int i = 0; i < objects_to_instantiate; i++)
        {
            GameObject instance = MonoBehaviour.Instantiate(_object, fields_points[UnityEngine.Random.Range(0, fields_points.Length - 1)], Quaternion.Euler(
            UnityEngine.Random.Range(0f, 360f), // Rotazione casuale sull'asse X
            UnityEngine.Random.Range(0f, 360f), // Rotazione casuale sull'asse Y
            UnityEngine.Random.Range(0f, 360f)  // Rotazione casuale sull'asse Z
            ));

            instantiated_objects.Add(instance); // Aggiungi l'istanza alla lista

        }

    }

    public void DecreasingObjects()
    {
        

        //for (int i = 0;i < lut.Length; i++) { Debug.Log($"{lut[i]}"); }

        int idx = Mathf.CeilToInt(Time.time / time_slot);


        // Correggi il controllo dell'indice
        if (idx >= lut.Length)
        {
            idx = lut.Length - 1; // Imposta al massimo indice valido
            //Debug.Log($"Hai raggiunto il massimo valore di indice per la LUT: {idx}");
        }

        current_object_number = objects_to_instantiate - lut[idx]; // Numero di oggetti da mantenere

        //Debug.Log($" Valore indice: {lut[idx]}, Corrent obj: {current_object_number},Oggetti in scena : {instantiated_objects.Count}");

        // Utilizza un ciclo inverso per evitare problemi di rimozione durante l'iterazione
        for (int i = 0; i < instantiated_objects.Count - 1; i++)
        {
            if (instantiated_objects[i] == null)
            {
                instantiated_objects.RemoveAt(i);
            }
            if (Mathf.Abs(Vector3.Distance(instantiated_objects[i].transform.position, cam.transform.position)) > 5 && current_object_number > 0)
            {

                MonoBehaviour.Destroy(instantiated_objects[i]); // Distruggi l'oggetto
                instantiated_objects.RemoveAt(i); // Rimuovi dalla lista
                current_object_number--;
                DestObj();

            }
        }
    }

    public void Spawning(int spawning)
    {

        for (int i = 0; i < spawning; i++)
        {

            GameObject instance = MonoBehaviour.Instantiate(_object, fields_points[UnityEngine.Random.Range(0, fields_points.Length - 1)], Quaternion.Euler(
            UnityEngine.Random.Range(0f, 360f), // Rotazione casuale sull'asse X
            UnityEngine.Random.Range(0f, 360f), // Rotazione casuale sull'asse Y
            UnityEngine.Random.Range(0f, 360f)  // Rotazione casuale sull'asse Z
            ));
        }

    }


    public void DestoyObjects(GameObject _destroy)
    {
        objects_to_instantiate--;
        instantiated_objects.Remove(_destroy);
    }


    //Funzioni private//////////////////////////
    private void GetAreaVertices(GameObject area)
    {
        if (area == null)
        {
            Debug.LogError("area is null");
        }

        MeshFilter mf_area = area.GetComponent<MeshFilter>();
        Mesh mesh_area = mf_area.sharedMesh;
        areas_points = mesh_area.vertices;

        for (int i = 0; i < areas_points.Length; i++)
        {
            areas_points[i] = area.transform.TransformPoint(areas_points[i]);
        }
    }

    private void GetPlaneVertices(MeshFilter plane)
    {
        if (plane == null)
        {
            Debug.LogError("vertices array is null");
        }
        Mesh mesh_plane = plane.sharedMesh;
        planes_points = mesh_plane.vertices;

        for(int i = 0;i < planes_points.Length; i++)
        {
            planes_points[i] = plane.transform.TransformPoint(planes_points[i]);
        }
    }

    private void GetFieldPoint(Vector3[] planes_points, Vector3[] fields_points, Dictionary<int, bool> tmp_dict)
    {

        int i = 0;
        foreach (var t in tmp_dict)
        {
            if (t.Value == true)
            {
                fields_points[i] = planes_points[t.Key] + RandVect3();
                i++;
            }
        }
    }

    private void GetNumFieldPoints()
    {
        foreach (var t in tmp_dict)
        {
            if (t.Value == true)
            {
                num_fields_points++;
            }
        }
    }

    private void DestObj()
    {
        objects_to_instantiate--;
    }

    private void LutInit()
    {
        lut = new int[objects_to_instantiate / time_slot];

        for (int i = 0; i < lut.Length; i++)
        {
            lut[i] =  objects_to_instantiate - (i * decrement_factor);
        }
    }


    private Vector3 RandVect3()
    {
       Vector3 vect = new Vector3(UnityEngine.Random.Range(0f, 1),
                                   0,
                                   UnityEngine.Random.Range(0f, 1));
        return vect;
    }


    //Getter e Setter 


    public Vector3[] Field
    {
        get { return fields_points; }
    }


}

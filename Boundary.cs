using Unity.VisualScripting;
using UnityEngine;

public class Boundary : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            other.transform.position += -1 * (transform.forward * 3);
            //Debug.Log("funziona----------------------------------------------------------------------------------------------");
            
        }
    }
}

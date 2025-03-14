using UnityEngine;

public class Collider_hands : MonoBehaviour
{
    // Start is called before the first frame update


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("onTriggerEnter is working");
        if (other.gameObject.tag == "feed") {
            Destroy(other.gameObject); 
            feedField1.Instance.DestroyObject(other.gameObject);
            playerStat.Instance.life_points += 2;
        }
        else if (other.gameObject.tag == "trash")
        {
            playerStat.Instance.life_points -= 5;
            Destroy(other.gameObject);
        }
        else
        {
            return;
        }

    }
}

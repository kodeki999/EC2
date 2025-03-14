using UnityEngine;
using UnityEngine.UI; // Importa il namespace UI
using TMPro;

public class UpdateUIText : MonoBehaviour
{
    public TMP_Text temperature; // Collegalo nell'Inspector
    public TMP_Text year;
    double temp = 19;
    int ye = 1960;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2f)
        {
            temp += 0.01;
            ye += 1;
            timer = 0f;
        }
        temperature.text = temp.ToString("F2") + "°" ;
        year.text = ye.ToString();

    }
}

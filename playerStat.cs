using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerStat : MonoBehaviour
{


    public static playerStat Instance;
    public Slider slider;
    public int life_points;
    public int max_life = 100;

    public void SetHealth(int life)
    {
        slider.value = life;
    }


    private void Awake()
    {

        Instance = this;
        slider.maxValue = max_life;
        slider.value = max_life;
    }



    void Start()
    {
        life_points = max_life;
    }

    // Update is called once per frame
    void Update()
    {
        SetHealth(life_points);
    }
}

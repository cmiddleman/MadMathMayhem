using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberToggleController : MonoBehaviour
{
    public const int MIN_TOGGLE_COUNT = 3;
    public int num;
    public Toggle toggle;
    private static int toggleCount = 0;


    void Start()
    {
        

        toggle.isOn = Settings.Instance.IsNumValid(num);
        toggleCount = Settings.Instance.ValidNumsCount();
    }

   
}

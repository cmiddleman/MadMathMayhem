using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpToggleController : MonoBehaviour
{
    public int op;
    public const int MIN_TOGGLE_COUNT = 1;
    public Toggle toggle;
    private static int toggleCount;


    private void Start()
    {
        toggleCount = Settings.Instance.ValidOpCount();
        toggle.isOn = Settings.Instance.IsOpValid(op);
    }
    public void OnToggle(bool value)
    {
        if (value)
        {
            toggleCount++;
        }
        else
        {
            toggleCount--;
            if (toggleCount < MIN_TOGGLE_COUNT)
            {
                toggle.isOn = true;
                Debug.Log("Select at least one operation!");
                return;
            }

        }
        Debug.Log(op.ToString() + ", " + toggleCount);
        Settings.Instance.SetOp(op, value);
    }
}

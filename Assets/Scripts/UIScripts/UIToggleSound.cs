using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UIToggleSound : MonoBehaviour
{
    private Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle = gameObject.GetComponent<Toggle>();
        toggle.onValueChanged.AddListener((value) => PlaySound());

    }

    //NOTE: only one click at once, to make toggle group buttons playing double issues, potentially lazy.
    void PlaySound()
    {
        AudioManager.Instance.Play("Click");
    }

}

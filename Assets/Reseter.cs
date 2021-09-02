using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reseter : MonoBehaviour
{
    public void Reset()
    {
        Settings.Instance.ResetData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

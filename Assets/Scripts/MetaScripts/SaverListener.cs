using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaverListener : MonoBehaviour
{
   public void SaveViaButton()
    {
        Settings.Instance.Save();
    }

   public void SaveViaToggle(bool b)
    {
        if (b)
            Settings.Instance.Save();
    }
}

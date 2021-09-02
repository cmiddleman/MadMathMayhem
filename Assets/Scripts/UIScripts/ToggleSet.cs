using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Custom UI class that relies on the existence of Settings class.
//Warning: will not work if min and max are the same. Currently will turn a toggle back on if its lower than minOn and off its larger than maxOn.
//Warning: if toggle is turned off and then back on both calls will fire events, I need to make it so it triggers neither...
public class ToggleSet : MonoBehaviour
{
    public int minOn;
    // If setName changes, it must also be manually changed in the settings SetNames array.
    public string setName;
    //To ignore maximum, leave as 0 or set to negative (for maximum emphasis of irrelevance!)
    public int maxOn;
    public Toggle[] toggles;
    public bool highlightable;
    private float holdTime;

    

    // Start is called before the first frame update
    void Start()
    {
        holdTime = 0;
        Settings.Instance.LoadToggleSetValues(this);

        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i; // fixes the "closure problem," whatever the f*** that is.
            toggles[index].onValueChanged.AddListener( (value) =>
            {
                ToggleValueChanged(index, value);
            });
        }

       
    }

    void ToggleValueChanged(int index, bool value)
    {

       
        int count = CountOn();
        if(count < minOn)
        {
            SetToggle(index, true, true);
            return;
        }
        else if(maxOn > 0 && count > maxOn)
        {
            SetToggle(index, false, true);
            return;
        }
        UpdateSettings();
    }

    public bool[] GetValues()
    {
        bool[] values = new bool[toggles.Length];
        for(int i = 0; i < values.Length; i++)
        {
            values[i] = toggles[i].isOn;
        }

        return values;
    }
    public int CountOn()
    {
        int count = 0;
        for (int i = 0; i < toggles.Length; i++)
            if (toggles[i].isOn)
                count++;

        return count;
    }

    //bool mute controls whether or not the Toggle.OnValueChanged event is called, default is false
    public void SetToggle(int index, bool value, bool mute)
    {
        if (mute)
        {
            MuteEventCalls(toggles[index].onValueChanged);
            
        }
        
        toggles[index].isOn = value;
       
        if(mute)
            UnmuteEventCalls(toggles[index].onValueChanged);
    }

    public void SetToggle(int index, bool value)
    {
        SetToggle(index, value, false);
    }

    void UpdateSettings()
    {
        Settings.Instance.UpdateToggleSetValues(this);
    }

    public void LoadValues(bool[] values)
    {
        if (values.Length != toggles.Length)
            Debug.LogError("Tried to load values of " + setName + " with values that is wrong length!");
       
        for(int i = 0; i < toggles.Length; i++)
        {
            SetToggle(i, values[i], true);
        }
    }

    void MuteEventCalls(UnityEngine.Events.UnityEventBase ev)
    {
        int count = ev.GetPersistentEventCount();
        for (int i = 0; i < count; i++)
        {
            ev.SetPersistentListenerState(i, UnityEngine.Events.UnityEventCallState.Off);
        }
    }

    void UnmuteEventCalls(UnityEngine.Events.UnityEventBase ev)
    {
        int count = ev.GetPersistentEventCount();
        for (int i = 0; i < count; i++)
        {
            ev.SetPersistentListenerState(i, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
        }
        
        
    }

}

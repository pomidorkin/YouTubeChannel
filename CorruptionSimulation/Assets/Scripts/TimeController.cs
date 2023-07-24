using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public float timeScale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        SetTimeSpeed(timeScale);
    }

    private void SetTimeSpeed(float val)
    {
        //Time.timeScale = val;
        iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", val, "time", 10, "onupdatetarget", gameObject, "onupdate", "UpdateCounter"));
    }

    void UpdateCounter(float newValue)
    {
        Time.timeScale = newValue;
    }
}

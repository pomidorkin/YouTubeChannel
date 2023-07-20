using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] int maxAvailableRes = 30;
    private int resourceCounter = 0;
    [SerializeField] AltruistController altruistController;

    private void OnEnable()
    {
        altruistController.ResetCommand += ResetResourceAmount;
    }

    private void OnDisable()
    {
        altruistController.ResetCommand -= ResetResourceAmount;
    }

    private void ResetResourceAmount()
    {
        resourceCounter = 0;
    }

    public void IncreaseResourceCounter(int val)
    {
        if ((resourceCounter + val) <= maxAvailableRes)
        {
            resourceCounter += val;

        }
        else
        {
            resourceCounter = maxAvailableRes;
        }
        
    }

    public bool MaxResReached()
    {
        return resourceCounter >= maxAvailableRes;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalistResourceManager : MonoBehaviour
{
    [SerializeField] int maxAvailableRes = 30;
    private int resourceCounter = 0;
    [SerializeField] CapitalistController capitalistController;

    private void OnEnable()
    {
        capitalistController.ResetCommand += ResetResourceAmount;
    }

    private void OnDisable()
    {
        capitalistController.ResetCommand -= ResetResourceAmount;
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

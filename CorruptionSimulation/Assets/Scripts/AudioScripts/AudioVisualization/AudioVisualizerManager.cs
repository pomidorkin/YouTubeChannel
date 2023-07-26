using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizerManager : MonoBehaviour
{
    [SerializeField] [Range(0, 7)] int band = 0;
    [SerializeField] [Range(0f, 10f)] float animTriggerValue = 1.6f;
    //[SerializeField] private MMFeedbacks MMFeedbacks;
    float elapsedTime;
    [SerializeField] float timeLimit = 0.1f;

    public delegate void PeakReachedAction();
    public event PeakReachedAction OnPeakReachedAction;
    private void Start()
    {
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= timeLimit && AudioPeer.bandBuffer[band] > animTriggerValue)
        {
            elapsedTime = 0;
            OnPeakReachedAction();
            //RaiseDoorSlammedEvent();
        }
    }

    /*public void RaiseDoorSlammedEvent()
    {
        if (MMFeedbacks != null)
        {
            MMFeedbacks.PlayFeedbacks();
        }
    }

    public void SetMMFeedbacks(MMFeedbacks mmf)
    {
        MMFeedbacks = mmf;
    }*/
}
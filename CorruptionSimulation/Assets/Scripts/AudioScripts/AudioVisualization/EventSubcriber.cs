using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSubcriber : MonoBehaviour
{
    [SerializeField] AudioVisualizerManager visualizerManager;
    [SerializeField] Animator animator;
    private void OnEnable()
    {
        visualizerManager.OnPeakReachedAction += Handler;
    }

    private void OnDisable()
    {
        visualizerManager.OnPeakReachedAction -= Handler;
    }

    private void Handler()
    {
        animator.Play("PropJump");
    }
}

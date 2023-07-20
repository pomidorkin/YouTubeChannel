using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TargetLookController : MonoBehaviour
{
    private bool lookAtTarget = true;
    [SerializeField] MultiAimConstraint multiAimConstraint;

    private void OnEnable()
    {
        TweenTargetLookSetter();
    }
    void TweenTargetLookSetter()
    {
        if (lookAtTarget)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 2, "onupdatetarget", gameObject, "onupdate", "UpdateCounter"));
        }
        else
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", 2, "onupdatetarget", gameObject, "onupdate", "UpdateCounter"));
        }
        this.lookAtTarget = !lookAtTarget;
    }

    void UpdateCounter(float newValue)
    {
        multiAimConstraint.weight = newValue;
    }
}

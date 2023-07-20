using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    [Serializable]
    public class FacialAnimPlayableBehavior : PlayableBehaviour
    {
        public FacialExpression facialExpression;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Debug.Log("ProcessFrame, " + facialExpression);

            FacialExpressionHandler facialExpressionPosition = playerData as FacialExpressionHandler;
            if (facialExpressionPosition != null)
            {
                facialExpressionPosition.FacialExpression = facialExpression;
            }
        }
    }
}

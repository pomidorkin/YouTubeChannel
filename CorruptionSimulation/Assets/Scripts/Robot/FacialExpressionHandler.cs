using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialExpressionHandler : MonoBehaviour
{
    // Similar to RhubarbSprite.cs

    [SerializeField] Material face;
    [SerializeField] private FacialAnimationsSet facialAnimsSet;

    public FacialAnimationsSet FacialAnimationsSet
    {
        get { return facialAnimsSet; }
        set { facialAnimsSet = value; }
    }

    public FacialExpression FacialExpression
    {
        set
        {
            face.mainTexture = (facialAnimsSet.GetFace(value));
        }
    }
}

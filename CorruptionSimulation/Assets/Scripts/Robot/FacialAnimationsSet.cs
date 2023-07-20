using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FacialAnimations/FacialAnim Set")]
public class FacialAnimationsSet : ScriptableObject
{
    [SerializeField] private Texture Angry;
    [SerializeField] private Texture Dead;
    [SerializeField] private Texture Disgust;
    [SerializeField] private Texture Evil;
    [SerializeField] private Texture Happy;
    [SerializeField] private Texture Normal;
    [SerializeField] private Texture Surprised;
    [SerializeField] private Texture Kawai;
    [SerializeField] private Texture Dollar;

    public Texture GetFace(FacialExpression facialExpression)
    {
        switch (facialExpression)
        {
            case FacialExpression.Angry:
                return Angry;
            case FacialExpression.Dead:
                return Dead;
            case FacialExpression.Disgust:
                return Disgust;
            case FacialExpression.Evil:
                return Evil;
            case FacialExpression.Happy:
                return Happy;
            case FacialExpression.Normal:
                return Normal;
            case FacialExpression.Surprised:
                return Surprised;
            case FacialExpression.Kawai:
                return Kawai;
            case FacialExpression.Dollar:
                return Dollar;
        }

        return Normal;
    }
}

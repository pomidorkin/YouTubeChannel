using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Timeline
{
    [TrackBindingType(typeof(FacialExpressionHandler))]
    [TrackClipType(typeof(FacialAnimPlayableClip))]
    public class FacialAnimationPlayableTrack : TrackAsset
    { }
}

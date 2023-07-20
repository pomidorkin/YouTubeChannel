using UnityEngine;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    public class FacialAnimPlayableClip : PlayableAsset
    {
        public FacialExpression facialExpression;


        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<FacialAnimPlayableBehavior>.Create(graph);
            FacialAnimPlayableBehavior facialAnimPlayableBehavior = playable.GetBehaviour();
            facialAnimPlayableBehavior.facialExpression = facialExpression;

            return playable;
        }
    }
}
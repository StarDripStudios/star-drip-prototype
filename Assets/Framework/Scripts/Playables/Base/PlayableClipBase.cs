using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Framework.Playables
{
    [Serializable]
    public abstract class PlayableClipBase<TBehaviour> : PlayableAsset, ITimelineClipAsset
        where TBehaviour : PlayableBehaviourBase, new()
    {
        public ClipCaps clipCaps => ClipCaps.None;
        public TimelineClip OwnedClip { get; set; }
        public UnityEngine.Object AttachObject { get; set; }

        public TBehaviour template = new TBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TBehaviour>.Create(graph, template);
            TBehaviour clone = playable.GetBehaviour();

            clone.AttachObject = AttachObject;
            clone.OwnedClip = OwnedClip;

            return playable;
        }
    }
}

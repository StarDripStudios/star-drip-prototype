using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Framework.Playables
{
    public abstract class PlayableTrackBase<TBehaviour, TMixer, TClip> : TrackAsset
        where TBehaviour : PlayableBehaviourBase, new()
        where TMixer : PlayableMixerBehaviourBase<TBehaviour>, new()
        where TClip : PlayableClipBase<TBehaviour>, new()
    {
        public Object AttackObject;

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject gameObject, int inputCount)
        {
            var director = gameObject.GetComponent<PlayableDirector>();
            AttackObject = director.GetGenericBinding(this);

            foreach (var clip in GetClips())
            {
                var playableAsset = clip.asset as TClip;
                if (playableAsset == null) continue;
                playableAsset.AttachObject = AttackObject;
                playableAsset.OwnedClip = clip;
            }

            return ScriptPlayable<TMixer>.Create(graph, inputCount);
        }
    }
}

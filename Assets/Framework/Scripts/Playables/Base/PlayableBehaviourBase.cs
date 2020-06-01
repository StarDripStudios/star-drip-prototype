using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Framework.Playables
{
    public class PlayableBehaviourBase : PlayableBehaviour
    {
        public TimelineClip OwnedClip { get; set; }
        public Object AttachObject { get; set; }
        public bool IsPlayHeadInside { get; private set; }

        public bool CanStart(float time)
        {
            if (OwnedClip == null) return false;
            if (time >= OwnedClip.start &&
                time <= OwnedClip.end &&
                !IsPlayHeadInside)
            {
                return true;
            }

            return false;
        }

        public bool IsPlaying(float time)
        {
            if (OwnedClip == null) return false;
            if (time >= OwnedClip.start &&
                time <= OwnedClip.end)
            {
                return true;
            }

            return false;
        }

        public bool IsDone(float time)
        {
            if (OwnedClip == null) return false;
            if (time >= OwnedClip.end &&
                IsPlayHeadInside)
            {
                return true;
            }

            return false;
        }

        public virtual void OnEnter(Playable playable, int i)
        {
            IsPlayHeadInside = true;
        }

        public virtual void OnStay(Playable playable, int i, float time)
        {
        }

        public virtual void OnExit(Playable playable, int i)
        {
            IsPlayHeadInside = false;
        }

    }
}

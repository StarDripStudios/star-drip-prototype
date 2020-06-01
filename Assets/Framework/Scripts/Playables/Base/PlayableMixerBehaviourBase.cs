using UnityEngine;
using UnityEngine.Playables;

namespace Framework.Playables
{
    public class PlayableMixerBehaviourBase<T> : PlayableBehaviour where T : PlayableBehaviourBase, new()
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (!Application.isPlaying) return; // This should only happen in play mode.
            if (info.deltaTime <= 0) return;

            var time = (float) playable.GetGraph().GetRootPlayable(0).GetTime();
            var duration = (float) playable.GetGraph().GetRootPlayable(0).GetDuration();

            var inputCount = playable.GetInputCount();
            for (var i = 0; i < inputCount; i++)
            {
                var inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<T>) playable.GetInput(i);
                var behaviour = inputPlayable.GetBehaviour();

                if (inputWeight >= 1)
                {
                    if (behaviour.CanStart(time))
                    {
                        //HasEntered
                        behaviour.OnEnter(playable, i);
                    }
                    else if (time >= duration)
                    {
                        //Has exited at the end of a timeline
                        behaviour.OnExit(playable, i);
                    }
                }
                else if (inputWeight <= 0 && behaviour.IsDone(time))
                {
                    //Has exited
                    behaviour.OnExit(playable, i);
                }

                if (inputWeight <= 0) continue;

                if (behaviour.IsPlaying(time))
                {
                    //Update
                    behaviour.OnStay(playable, i, time);
                }
            }
        }
    }
}

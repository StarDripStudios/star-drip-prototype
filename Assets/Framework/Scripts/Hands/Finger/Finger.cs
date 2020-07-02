using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Framework
{
    [System.Serializable]
    public class Finger
    {
        AnimationLayerMixerPlayable mixer;
        [SerializeField, Range(0, 1)] private float weight;
        private float lastweight;
        private CrossFadingFloat crossFadingWeight;

        public float Weight
        {
            get => weight;
            set
            {
                if (Mathf.Abs(value - lastweight) <= .001f) return;

                value = Mathf.Clamp01(value);
                weight = value;
                lastweight = value;
                crossFadingWeight.Value = value;
            }
        }
        
        public AnimationLayerMixerPlayable Mixer => mixer;

        public Finger(PlayableGraph graph, AnimationClip closed, AnimationClip opened, AvatarMask mask)
        {
            mixer = AnimationLayerMixerPlayable.Create(graph, 2);
            
            var openPlayable = AnimationClipPlayable.Create(graph, opened);
            graph.Connect(openPlayable, 0, mixer, 0);
            
            var closedPlayable = AnimationClipPlayable.Create(graph, closed);
            graph.Connect(closedPlayable, 0, mixer, 1);
            
            mixer.SetLayerAdditive(0, false);
            mixer.SetLayerMaskFromAvatarMask(0, mask);
            mixer.SetInputWeight(0, 1);
            mixer.SetInputWeight(1, 0);
            
            crossFadingWeight = new CrossFadingFloat();
            crossFadingWeight.OnChange += (value) =>
            {
                mixer.SetInputWeight(0, 1 - value);
                mixer.SetInputWeight(1, value);
            };
        }
    }
    
    [Flags]
    public enum FingerName
    {
        None = -1,
        Thumb = 0,
        Index = 1,
        Middle = 2,
        Ring = 3,
        Pinky = 4,
    }
}
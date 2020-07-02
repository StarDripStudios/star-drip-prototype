using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Framework
{
    [System.Serializable]
    public class Pose
    {
        public AnimationClipPlayable playable;
        public AnimationClip clip;

        private CrossFadingFloat _crossFader;

        public float Weight { get; private set; }

        public void SetWeight(float value)
        {
            if (_crossFader == null)
            {
                _crossFader = new CrossFadingFloat();
                _crossFader.OnChange += v =>
                {
                    Weight = v;
                };
            }
            _crossFader.Value = value;
        }
    }
    
    public class HandAnimationController : MonoBehaviour
    {
        [HideInInspector, SerializeField] private HandData handData;
        [HideInInspector, SerializeField] private Finger[] fingers;
        [HideInInspector, SerializeField] public PlayableGraph graph;
        [HideInInspector, SerializeField] private bool staticPose;
        [HideInInspector, SerializeField] private List<Pose> poses;
        [HideInInspector, SerializeField] private int pose;
        
        private CrossFadingFloat _staticPoseCrossFader;
        private AnimationMixerPlayable _handMixer;
        private bool _isInitialized;
        private AnimationMixerPlayable _poseMixer;

        public bool StaticPose
        {
            get => staticPose;
            set
            {
                if (value != staticPose)
                {
                    if (_staticPoseCrossFader == null)
                    {
                        _staticPoseCrossFader = new CrossFadingFloat();
                        _staticPoseCrossFader.OnChange += (v) => {
                            _handMixer.SetInputWeight(0, 1-v);
                            _handMixer.SetInputWeight(1, v);
                        };
                    }
                    _staticPoseCrossFader.Value = (value) ? 1 : 0;
                    
                }
                staticPose = value;
            }
        }

        public int Pose
        {
            get => pose;
            set
            {
                if (pose != value)
                {
                    poses[pose].SetWeight(0);
                    poses[value].SetWeight(1);
                }
                pose = value;
            }
        }

        public HandData HandData => handData; 
        public bool IsInitialized => _isInitialized; 

        public float this[FingerName index]
        {
            get => fingers.Length != 0 ? fingers[(int)index].Weight : (int)FingerName.None;
            set
            {
                if (fingers.Length == 0) return;
                fingers[(int) index].Weight = value;
            }
        }
        
        public float this[int index]
        {
            get => fingers.Length != 0 ? fingers[index].Weight : 1;
            set
            {
                if (fingers.Length == 0) return;
                fingers[index].Weight = value;
            }
        }

        public void Initialize()
        {
            if (handData)
            {
                graph = PlayableGraph.Create("Hand Animation Controller graph");
                fingers = new Finger[5];
                var fingerMixer = AnimationLayerMixerPlayable.Create(graph, fingers.Length);
                for (uint i = 0; i < fingers.Length; i++)
                {
                    fingers[i] = new Finger(graph, handData.closed, handData.opened, handData[(int) i]);
                    fingerMixer.SetLayerAdditive(i, false);
                    fingerMixer.SetLayerMaskFromAvatarMask(i, handData[(int) i]);
                    graph.Connect(fingers[i].Mixer, 0, fingerMixer, (int) i);
                    fingerMixer.SetInputWeight((int) i, 1);
                }

                _handMixer = AnimationMixerPlayable.Create(graph, 2);
                graph.Connect(fingerMixer, 0, _handMixer, 0);
                _handMixer.SetInputWeight(0, 1);
                if (handData.poses.Count > 0)
                {
                    _poseMixer = AnimationMixerPlayable.Create(graph, handData.poses.Count);
                    poses = new List<Pose>();
                    for (int i = 0; i < handData.poses.Count; i++)
                    {
                        var poseClip = handData.poses[i];
                        if (poseClip)
                        {
                            var pose = new Pose();
                            pose.playable = AnimationClipPlayable.Create(graph, handData.poses[i]);
                            pose.clip = handData.poses[i];
                            poses.Add(pose);
                            graph.Connect(pose.playable, 0, _poseMixer, i);
                            _poseMixer.SetInputWeight(i, 0);
                        }

                        _poseMixer.SetInputWeight(pose, 1);
                    }

                    if (poses.Count > 0)
                    {
                        graph.Connect(_poseMixer, 0, _handMixer, 1);
                        _handMixer.SetInputWeight(1, 0);
                    }
                }

                var playableOutput =
                    AnimationPlayableOutput.Create(graph, "Hand Controller", GetComponentInChildren<Animator>());
                playableOutput.SetSourcePlayable(_handMixer);
                _isInitialized = true;
                graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
                graph.Play();

            }
            else
            {
                _isInitialized = false;
            }
        }

        private void Start()
        {
            if (!_isInitialized)
            {
                Initialize();
            }
            graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        }
        
        private void OnDisable()
        {
            graph.Destroy();
        }
        
        public void Update()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                graph.Evaluate();
            }
#endif
            graph.Evaluate();

            for (var i = 0; i < poses.Count; i++)
            {
                try
                {
                    _poseMixer.SetInputWeight(i, poses[i].Weight);
                }
                catch
                {
                    poses.RemoveAt(i);
                }
            }
        }
    }
}
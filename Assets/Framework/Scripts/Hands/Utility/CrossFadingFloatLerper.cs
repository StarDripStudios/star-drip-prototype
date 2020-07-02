using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class CrossFadingFloat
    {
        public event Action<float> OnChange;
        private float _start;
        private float _target;
        private float _value;
        private float _rate;
        private float _time;

        public float Value
        {
            get => _value; 
            set
            {
#if UNITY_EDITOR
                if (UnityEditor.EditorApplication.isPlaying)
#endif
                {

                    _time = 0;
                    _start = _value;
                    _target = value;
                    CrossFadingFloatLerper.Instance.AddCrossFadingFloat(this);
                }
#if UNITY_EDITOR
                else
                {
                    _value = value;
                    OnChange(value);
                }
#endif
            }
        }

        public CrossFadingFloat(float rate = 5f, float value = 0)
        {
            _start = _target = _value = value;
            _rate = rate;
            _time = 1;
            OnChange = null;
        }

        public bool Step()
        {
            _time += _rate * Time.deltaTime;

            _value = Mathf.Lerp(_start, _target, _time);
            OnChange?.Invoke(_value);
            return _time >= 1;
        }
    }

    public class CrossFadingFloatLerper : MonoBehaviour
    {
        private static List<CrossFadingFloat> values;
        private static CrossFadingFloatLerper instance;

        public static CrossFadingFloatLerper Instance
        {
            get
            {
                if (instance) return instance;
                
                instance = new GameObject("CrossFadingFloatLerper").AddComponent<CrossFadingFloatLerper>();
                values = new List<CrossFadingFloat>();
                DontDestroyOnLoad(instance);

                return instance;
            }
        }

        public void AddCrossFadingFloat(CrossFadingFloat value)
        {
            values.Add(value);
        }

        private void Update()
        {
            for (var i = values.Count - 1; i >= 0; i--)
            {
                if (values[i].Step())
                {
                    values.RemoveAt(i);
                }
            }
        }
    }
}

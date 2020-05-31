using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Ixion.Prototype
{
    public class CompressInteraction : Interaction
    {
        public Transform target;
        public Collider bounds;
        [Range(0.1f, 1)]
        public float validPressRange = 0.5f;
        public float compressionDuration = 5;
        public UnityEvent onButtonPressed;

        private float _min, _max;
        public float _timeDelta;
        private bool _isInside;
        private bool _wasPressed;

        private void Start()
        {
            _max = target.localPosition.y;
            _min = _max - bounds.bounds.size.y * validPressRange;
            _timeDelta = Mathf.Abs(_min - _max) / compressionDuration;
        }

        private void Update()
        {
            if (!HoverInteractor ||
                !_isInside) return;

            var result = Mathf.MoveTowards(target.localPosition.y, _min, Time.deltaTime * _timeDelta);
            SetYPosition(result);

            var isPressed = IsPressed();
            if (isPressed &&
                !_wasPressed)
            {
                onButtonPressed?.Invoke();
                _wasPressed = true;
                Debug.Log("<color=yellow>Compression interaction complete</color>");
            }
        }

        public override void OnInteractionEnter(XRBaseInteractor interactor)
        {
            _isInside = true;
            
            base.OnInteractionEnter(interactor);
        }

        public override void OnInteractionExit(XRBaseInteractor interactor)
        {
            SetYPosition(_max);
            _isInside = false;
            _wasPressed = false;
            
            base.OnInteractionExit(interactor);
        }
        
        private float GetLocalYPosition(Vector3 position)
        {
            var localPosition = target.parent.InverseTransformVector(position);
            return localPosition.y;
        }

        private void SetYPosition(float value)
        {
            var newPosition = target.localPosition;
            newPosition.y = Mathf.Clamp(value, _min, _max);
            target.localPosition = newPosition;
        }

        private bool IsPressed()
        {
            var yValue = target.localPosition.y;
            var range = Mathf.Clamp(yValue, _min, _min);
            return yValue == range;
        }
    }
}

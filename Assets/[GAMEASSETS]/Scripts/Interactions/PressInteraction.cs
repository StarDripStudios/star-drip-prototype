using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Ixion.Prototype
{
    public class PressInteraction : Interaction
    {
        public Transform target;
        public Collider collision;
        public UnityEvent onButtonPressed;

        private float _previousHandHeight;
        private float _min, _max;
        private bool _wasPressed;

        private void Start()
        { 
            _max = target.localPosition.y;
            _min = _max - collision.bounds.size.y * 0.5f;
        }

        public override void OnInteractionEnter(XRBaseInteractor interactor)
        {
            _previousHandHeight = GetLocalYPosition(interactor.transform.localPosition);

            base.OnInteractionEnter(interactor);
        }

        public override void OnInteractionExit(XRBaseInteractor interactor)
        {
            _previousHandHeight = 0f;
            SetYPosition(_max);
            _wasPressed = false;
            base.OnInteractionExit(interactor);
        }

        private void Update()
        {
            if (!HoverInteractor) return;

            var newHandHeight = GetLocalYPosition(HoverInteractor.transform.localPosition);
            var difference = _previousHandHeight - newHandHeight;
            _previousHandHeight = newHandHeight;

            var newHeightValue = target.localPosition.y - difference;
            SetYPosition(newHeightValue);

            CheckPress();
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

        private void CheckPress()
        {
            var isPressed = IsPressed();
            if (isPressed &&
                !_wasPressed)
            {
                onButtonPressed?.Invoke();
            }

            _wasPressed = isPressed;
        }

        private bool IsPressed()
        {
            var yValue = target.localPosition.y;
            var range = Mathf.Clamp(yValue, _min, _min + 0.01f);
            return yValue == range;
        }
    }
}

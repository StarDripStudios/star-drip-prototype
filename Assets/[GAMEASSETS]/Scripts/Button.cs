using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Ixion.Prototype
{
    public class Button : XRBaseInteractable
    {
        public UnityEvent onButtonPressed;

        private float _min;
        private float _max;
        private bool _wasPressed;

        private float _previousHandHeight;
        private XRBaseInteractor _hoverInteractor;

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();

            onHoverEnter.AddListener(OnStartPress);
            onHoverExit.AddListener(OnEndPress);
        }

        private void Start()
        {
            var col = GetComponent<Collider>();
            _max = transform.localPosition.y;
            _min = _max - col.bounds.size.y * 0.5f;
        }

        private void OnDestroy()
        {
            onHoverEnter.RemoveListener(OnStartPress);
            onHoverExit.RemoveListener(OnEndPress);
        }

        #endregion MonoBehaviour

        private void OnStartPress(XRBaseInteractor interactor)
        {
            _hoverInteractor = interactor;
            _previousHandHeight = GetLocalYPosition(_hoverInteractor.transform.localPosition);
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            if (!_hoverInteractor) return;

            var newHandHeight = GetLocalYPosition(_hoverInteractor.transform.localPosition);
            var difference = _previousHandHeight - newHandHeight;
            _previousHandHeight = newHandHeight;

            var newHeightValue = transform.localPosition.y - difference;
            SetYPosition(newHeightValue);

            CheckPress();
        }

        private void OnEndPress(XRBaseInteractor interactor)
        {
            _hoverInteractor = null;
            _previousHandHeight = 0f;
            SetYPosition(_max);
            _wasPressed = false;
        }

        private float GetLocalYPosition(Vector3 position)
        {
            var localPosition = transform.parent.InverseTransformVector(position);
            return localPosition.y;
        }

        private void SetYPosition(float value)
        {
            var newPosition = transform.localPosition;
            newPosition.y = Mathf.Clamp(value, _min, _max);
            transform.localPosition = newPosition;
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
            var yValue = transform.localPosition.y;
            var range = Mathf.Clamp(yValue, _min, _min + 0.01f);
            return yValue == range;
        }
    }
}

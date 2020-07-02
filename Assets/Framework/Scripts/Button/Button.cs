using UnityEngine;
using UnityEngine.Events;

namespace Framework
{
    public class Button : MonoBehaviour
    {
        public LayerMask validInteraction;
        public Transform target;
        public GameObject collidingObject;
        [Range(0.1f, 1)] 
        public float validPressRange = 0.5f;
        public float releaseSpeed = 5;

        private bool _wasPressed;
        private float _min, _max;
        private float _difference;
        private Vector3 _original;
        private float _lastInteractionHeight;

        protected virtual void Start()
        {
            _original = target.localPosition;
            _max = _original.y;
            _min = _max - target.localScale.y * validPressRange;
        }

        protected virtual void Update()
        {
            if (!collidingObject )
            {
                // smoothly release button
                if (target.localPosition.y - _max < 0.01f)
                {
                    SetHeight(Mathf.Lerp(target.localPosition.y, _max, Time.deltaTime * releaseSpeed));
                }

                return;
            }

            var currentInteractionHeight = GetYPosition(collidingObject.transform);
            _difference = _lastInteractionHeight - currentInteractionHeight;
            _lastInteractionHeight = currentInteractionHeight;

            var newHeight = target.localPosition.y - _difference;
            SetHeight(newHeight);
            
            CheckPress();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (validInteraction != (validInteraction | (1 << other.gameObject.layer))) return;
            collidingObject = other.gameObject;

            _lastInteractionHeight = GetYPosition(collidingObject.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if (validInteraction != (validInteraction | (1 << other.gameObject.layer))) return;
            collidingObject = null;
            _wasPressed = false;
        }

        private float GetYPosition(Transform source)
        {
            var inverted = transform.InverseTransformPoint(source.position);
            return inverted.y;
        }

        private void SetHeight(float value)
        {
            var position = new Vector3
            {
                y = Mathf.Clamp(value, _min, _max)
            };

            target.localPosition = position;
        }

        private void CheckPress()
        {
            if (!(target.localPosition.y < _min + 0.01f) || _wasPressed) return;
            ButtonPress();
        }

        protected virtual void ButtonPress()
        {
            _wasPressed = true;
        }
    }
}

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Framework
{
    public class SmoothTurnProvider : LocomotionProvider
    {
        public XRRig rig;
        public XRController controller;
        public float speed;

        private bool _isInitialized;
        private Transform _rigTransform;

        private void Start()
        {
            if (!rig)
            {
                Debug.LogWarning("No XRRig reference on SmoothTurnProvider");
                return;
            }

            _rigTransform = rig.transform;

            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;
            
            CheckInput();
        }

        private void CheckInput()
        {
            if (!controller) return;
            if (!controller.enableInputActions) return;
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out var position))
            {
                Turn(position);
            }
        }

        private void Turn(Vector2 position)
        {
            var rotation = new Vector3
            {
                y = Time.deltaTime * position.x * speed
            };
            
            _rigTransform.Rotate(rotation);
        }
    }
}

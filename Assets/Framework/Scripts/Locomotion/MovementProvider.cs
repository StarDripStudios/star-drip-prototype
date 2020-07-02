using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Framework
{
    public class MovementProvider : LocomotionProvider
    {
        public XRRig rig;
        public Vector3 cameraPosition;
        public CharacterController characterController;
        public XRController controller;
        public float speed;
        public float gravityMultiplier;
        
        private bool _isInitialized;
        private Transform _headTransform;
        
        private void Start()
        {
            if (!rig)
            {
                Debug.LogWarning("No XRRig reference on MovementProvider.");
                return;
            }

            cameraPosition = rig.cameraInRigSpacePos;
            _headTransform = rig.cameraGameObject.transform;
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;
            
            CheckInput();
            ApplyGravity();
        }

        private void CheckInput()
        {
            if (!controller) return;
            if (!controller.enableInputActions) return;
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out var position))
            {
                Move(position);
            }
        }

        private void ApplyGravity()
        {
            var gravity = new Vector3
            {
                y = Physics.gravity.y * gravityMultiplier
            };

            gravity *= Time.deltaTime;
            gravity *= Time.deltaTime;
            characterController.Move(gravity);
        }

        private void Move(Vector2 position)
        {
            var right = _headTransform.right;
            var forward = _headTransform.forward;

            right *= position.x;
            forward *= position.y;

            var heading = right + forward;
            heading.y = 0;
            heading *= speed;
            heading *= Time.deltaTime;

            characterController.Move(heading);
        }
    }
}

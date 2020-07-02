using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Framework
{
    [RequireComponent(typeof(CharacterController))]
    public class BodyResizer : MonoBehaviour
    {
        public XRRig rig;
        public float maxHeight = 2;

        private bool _isInitialized;
        private CharacterController _characterController;
        private Transform _headTransform;

        private void Start()
        {
            if (!(_characterController = GetComponent<CharacterController>()))
            {
                Debug.LogWarning("Could not find CharacterController on BodyResizer.");
                return;
            }
            
            if (!rig)
            {
                Debug.LogWarning("No reference to XRRig on BodyResizer.");
                return;
            }

            _headTransform = rig.cameraGameObject.transform;

            _isInitialized = true;
            
            ResizeController();
        }

        private void Update()
        {
            if (!_isInitialized) return;
            
            ResizeController();
        }

        private void ResizeController()
        {
            var headPosition = _headTransform.localPosition;
            var headHeight = Mathf.Clamp(headPosition.y, 1, maxHeight);
            _characterController.height = headHeight;

            var center = new Vector3
            {
                y = _characterController.height * 0.5f,
            };
            center.y += _characterController.skinWidth;

            _characterController.center = center;
        }
    }
}

using UnityEngine;
using UnityEngine.XR;

namespace Framework
{
    [RequireComponent(typeof(Hand))]
    [RequireComponent(typeof(HandAnimationController))]
    public class HandInput : MonoBehaviour
    {
        /// <summary>
        /// the input manager is to get input from the diffrent buttons on the controllers
        /// </summary>
        public VRInputManager inputManager;
        private HandAnimationController animationController;
        private Hand hand;
        InputDevice device;
        
        private void Start()
        {
            animationController = GetComponent<HandAnimationController>();
            hand = GetComponent<Hand>();
            switch (hand.hand)
            {
                case Handedness.Right:
                    device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                    break;
                case Handedness.Left:
                    device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                    break;
                default:
                    break;
            }
        }
        
        private void Update()
        {
            UpdateFingers();
        }
        
        private void UpdateFingers()
        {
            for (var i = 0; i < 5; i++) 
            {
                var finger = (FingerName)i;
                var value = inputManager.GetFingerValue(hand.hand, finger);
                switch (finger)
                {
                    case FingerName.Thumb:
                        value = Mathf.Lerp(hand.Constraints.thumbFingerLimits.x, hand.Constraints.thumbFingerLimits.y, value);
                        break;
                    case FingerName.Index:
                        value = Mathf.Lerp(hand.Constraints.indexFingerLimits.x, hand.Constraints.indexFingerLimits.y, value);
                        break;
                    case FingerName.Middle:
                        value = Mathf.Lerp(hand.Constraints.middleFingerLimits.x, hand.Constraints.middleFingerLimits.y, value);
                        break;
                    case FingerName.Ring:
                        value = Mathf.Lerp(hand.Constraints.ringFingerLimits.x, hand.Constraints.ringFingerLimits.y, value);
                        break;
                    case FingerName.Pinky:
                        value = Mathf.Lerp(hand.Constraints.pinkyFingerLimits.x, hand.Constraints.pinkyFingerLimits.y, value);
                        break;
                }
                animationController[i] = value;
            }
        }
    }
}


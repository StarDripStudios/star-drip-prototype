using UnityEngine;

namespace Framework
{
    public enum ControllerType
    {
        Normal,
        Knuckles
    }

    [CreateAssetMenu(menuName = "Framework/InputManager")]
    public class VRInputManager : ScriptableObject
    {
        public ControllerType type;

        [Header("Left hand")] [HideInInspector] [SerializeField]
        private string leftTrigger = "Framework_left_trigger";

        [HideInInspector] [SerializeField] private string leftGrip = "Framework_left_grip";
        [HideInInspector] [SerializeField] private string leftThumb = "Framework_left_thumb";

        [Header("Left hand")] [HideInInspector] [SerializeField]
        private string leftIndex = "Framework_left_index";

        [HideInInspector] [SerializeField] private string leftMiddle = "Framework_left_middle";
        [HideInInspector] [SerializeField] private string leftRing = "Framework_left_ring";
        [HideInInspector] [SerializeField] private string leftPinky = "Framework_left_pinky";

        [Header("Right hand ")] [HideInInspector] [SerializeField]
        private string rightTrigger = "Framework_right_trigger";

        [HideInInspector] [SerializeField] private string rightGrip = "Framework_right_grip";

        [Header("Right hand")] [HideInInspector] [SerializeField]
        private string rightIndex = "Framework_right_index";

        [HideInInspector] [SerializeField] private string rightMiddle = "Framework_right_middle";
        [HideInInspector] [SerializeField] private string rightRing = "Framework_right_ring";
        [HideInInspector] [SerializeField] private string rightPinky = "Framework_right_pinky";
        [HideInInspector] [SerializeField] private string rightThumb = "Framework_right_thumb";

        public string LeftTrigger => leftTrigger;
        public string LeftGrip => leftGrip;
        public string LeftThumb => leftThumb;
        public string LeftIndex => leftIndex;
        public string LeftMiddle => leftMiddle;
        public string LeftRing => leftRing;
        public string LeftPinky => leftPinky;
        public string RightTrigger => rightTrigger;
        public string RightGrip => rightGrip;
        public string RightIndex => rightIndex;
        public string RightMiddle => rightMiddle;
        public string RightRing => rightRing;
        public string RightPinky => rightPinky;
        public string RightThumb => rightThumb;

        public bool RightGripDown => Input.GetAxis(rightGrip) > .2f;
        public bool RightTriggerDown => Input.GetAxis(rightTrigger) > .2f;
        public bool LeftGripDown => Input.GetAxis(leftGrip) > .2f;
        public bool LeftTriggerDown => Input.GetAxis(LeftTrigger) > .2f;

        public float GetFingerValue(Handedness hand, FingerName finger)
        {
            var value = 0f;
            switch (type)
            {
                case ControllerType.Normal:
                    value = GetNormalInput(finger, hand);

                    break;
                case ControllerType.Knuckles:
                    value = GetKnucklesInput(finger, hand);
                    break;
                default:
                    break;
            }

            return value;
        }

        private float GetNormalInput(FingerName finger, Handedness hand)
        {
            float value = 0;
            switch (finger)
            {
                case FingerName.Thumb:
                    if (hand == Handedness.Left)
                    {
                        value = Input.GetAxis(LeftThumb);
                        if (value < .1f)
                        {
                            if (Input.GetKey(KeyCode.JoystickButton8) ||
                                Input.GetKey(KeyCode.JoystickButton2) ||
                                Input.GetKey(KeyCode.JoystickButton3))
                            {
                                value = 1;
                            }
                            else if (Input.GetKey(KeyCode.JoystickButton16) ||
                                     Input.GetKey(KeyCode.JoystickButton12) ||
                                     Input.GetKey(KeyCode.JoystickButton13))
                            {
                                value = .5f;
                            }
                        }
                    }
                    else
                    {
                        value = Input.GetAxis(RightThumb);
                        if (value < .1f)
                        {
                            if (
                                Input.GetKey(KeyCode.JoystickButton9) ||
                                Input.GetKey(KeyCode.JoystickButton0) ||
                                Input.GetKey(KeyCode.JoystickButton1)
                            )
                            {
                                value = 1;
                            }
                            else if (
                                Input.GetKey(KeyCode.JoystickButton17) ||
                                Input.GetKey(KeyCode.JoystickButton10) ||
                                Input.GetKey(KeyCode.JoystickButton11)
                            )
                            {
                                value = .5f;
                            }
                        }
                    }

                    break;
                case FingerName.Index:
                    if (hand == Handedness.Left)
                    {
                        value = Input.GetAxis(leftTrigger);
                    }
                    else
                    {
                        value = Input.GetAxis(RightTrigger);
                    }

                    break;

                case FingerName.Middle:
                    if (hand == Handedness.Left)
                    {
                        value = Input.GetAxis(leftGrip);
                    }
                    else
                    {
                        value = Input.GetAxis(rightGrip);
                    }

                    break;
                case FingerName.Ring:
                    if (hand == Handedness.Left)
                    {
                        value = Input.GetAxis(leftGrip);
                    }
                    else
                    {
                        value = Input.GetAxis(rightGrip);
                    }

                    break;
                case FingerName.Pinky:
                    if (hand == Handedness.Left)
                    {
                        value = Input.GetAxis(LeftGrip);
                    }
                    else
                    {
                        value = Input.GetAxis(RightGrip);
                    }

                    break;
                default:
                    break;
            }

            return value;
        }

        private float GetKnucklesInput(FingerName finger, Handedness hand)
        {
            float value = 0;
            switch (finger)
            {
                case FingerName.Thumb:
                    if (hand == Handedness.Left)
                    {
                        value = Input.GetAxis(LeftThumb);
                        if (value < .1f)
                        {
                            if (Input.GetKey(KeyCode.JoystickButton8))
                            {
                                value = 1;
                            }
                            else if (Input.GetKey(KeyCode.JoystickButton16))
                            {
                                value = .5f;
                            }
                        }
                    }
                    else
                    {
                        value = Input.GetAxis(RightThumb);
                        if (value < .1f)
                        {
                            if (Input.GetKey(KeyCode.JoystickButton9))
                            {
                                value = 1;
                            }
                            else if (Input.GetKey(KeyCode.JoystickButton17))
                            {
                                value = .5f;
                            }
                        }

                        Debug.Log(value);
                    }

                    break;
                case FingerName.Index:
                    if (hand == Handedness.Left)
                    {
                        value = Input.GetAxis(LeftIndex);
                    }
                    else
                    {
                        value = Input.GetAxis(RightIndex);
                    }

                    break;

                case FingerName.Middle:
                    if (hand == Handedness.Left)
                    {
                        value = Input.GetAxis(LeftMiddle);
                    }
                    else
                    {
                        value = Input.GetAxis(RightMiddle);
                    }

                    break;
                case FingerName.Ring:
                    if (hand == Handedness.Left)
                    {
                        value = Input.GetAxis(LeftRing);
                    }
                    else
                    {
                        value = Input.GetAxis(RightRing);
                    }

                    break;
                case FingerName.Pinky:
                    if (hand == Handedness.Left)
                    {
                        value = Input.GetAxis(LeftPinky);
                    }
                    else
                    {
                        value = Input.GetAxis(RightPinky);
                    }

                    break;
                default:
                    break;
            }

            return value;
        }
    }
}
using System;
using TMPro;
using UnityEngine.Events;

namespace Framework
{
    public class KeyboardButton : Button
    {
        public string defaultValue;
        public string shiftValue;
        public TextMeshPro keyText;
        public KeyboardButtonPress onButtonPress;

        private string _currentKeyValue;

        protected override void Start()
        {
            var keyboard = GetComponentInParent<Keyboard>();
            if (keyboard) keyboard.onShiftPress += ShiftPress;

            _currentKeyValue = defaultValue;
            keyText.text = _currentKeyValue;
            
            base.Start();
        }

        private void ShiftPress(bool isPressed)
        {
            _currentKeyValue = isPressed ? shiftValue : defaultValue;
            keyText.text = _currentKeyValue;
        }

        protected override void ButtonPress()
        {
            onButtonPress?.Invoke(_currentKeyValue);
            
            base.ButtonPress();
        }
        
        [Serializable]
        public class KeyboardButtonPress : UnityEvent<string> { }
    }
}

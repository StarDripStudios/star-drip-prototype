using System;
using TMPro;
using UnityEngine;

namespace Framework
{
    public class Keyboard : MonoBehaviour
    {
        public event Action<bool> onShiftPress;

        [TextArea]
        public string entryValue;
        public TextMeshPro entryText;

        private bool _isShiftPressed = true;

        private void Start()
        {
            onShiftPress?.Invoke(_isShiftPressed);
        }

        public void KeyPressed(string value)
        {
            entryValue += value;
            entryText.text = entryValue;

            if (_isShiftPressed) ShiftPress();
        }

        public void ShiftPress()
        {
            _isShiftPressed = !_isShiftPressed;
            onShiftPress?.Invoke(_isShiftPressed);
        }

        public void EnterPressed()
        {
            Debug.Log($"Enter pressed with entry {entryValue}");
        }

        public void BackspacePressed()
        {
            if (entryValue.Length == 0) return;
            
            var remove = entryValue.Remove(entryValue.Length - 1, 1);
            entryValue = remove;
            entryText.text = entryValue;
        }
    }
}

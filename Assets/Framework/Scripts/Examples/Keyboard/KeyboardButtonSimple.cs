using UnityEngine.Events;

namespace Framework
{
    public class KeyboardButtonSimple : Button
    {
        public UnityEvent onButtonPress;
        
        protected override void ButtonPress()
        {
            onButtonPress?.Invoke();
            base.ButtonPress();
        }
    }
}

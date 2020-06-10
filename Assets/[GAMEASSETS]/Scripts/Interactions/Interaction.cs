using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace StarDust
{
    public class Interaction : MonoBehaviour
    {
        protected XRBaseInteractor HoverInteractor;
        
        public UnityEvent onHoverEnter;
        public UnityEvent onHoverExit;
        
        public virtual void OnInteractionEnter(XRBaseInteractor interactor)
        {
            HoverInteractor = interactor;
            onHoverEnter?.Invoke();
        }
        
        public virtual void OnInteractionExit(XRBaseInteractor interactor)
        {
            HoverInteractor = null;
            onHoverExit?.Invoke();
        }

        public virtual void Indicate()
        {
            
        }

        public virtual void ResetInteraction()
        {
            HoverInteractor = null;
            onHoverExit?.Invoke();
        }
    }

    public enum InteractionType
    {
        Press,
        Hold,
        Compress
    }
}

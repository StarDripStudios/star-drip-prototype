using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace StarDust
{
    public class Interaction : MonoBehaviour
    {
        protected XRBaseInteractor HoverInteractor;
        
        public virtual void OnInteractionEnter(XRBaseInteractor interactor)
        {
            HoverInteractor = interactor;
        }
        
        public virtual void OnInteractionExit(XRBaseInteractor interactor)
        {
            HoverInteractor = null;
        }

        public virtual void Indicate()
        {
            
        }
    }

    public enum InteractionType
    {
        Press,
        Hold,
        Compress
    }
}

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Ixion.Prototype
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
    }

    public enum InteractionType
    {
        Press,
        Hold,
        Compress
    }
}

using System;
using UnityEngine;

namespace StarDust
{
	public class InteractionContainer : MonoBehaviour
	{
		public Color defaultInteractionColor = Color.white;
		public Renderer interactionRenderer;
		public string interactionColorProperty;
		
		[Header("Interactions")]
		public PressInteraction pressInteraction;
		public HoldInteraction holdInteraction;
		public CompressInteraction compressionInteraction;

		private InteractionType _type;
		private Action<InteractionContainer, InteractionType> _onPressedCallback;
		private GameObject activeInteraction;

		public void Indicate(Color color)
		{
			if (!interactionRenderer ||
			    !interactionRenderer.material) return;
			
			interactionRenderer.material.SetColor(interactionColorProperty, color);
		}

		public void ResetIndication()
		{
			if (!interactionRenderer ||
			    !interactionRenderer.material) return;
			
			interactionRenderer.material.SetColor(interactionColorProperty, defaultInteractionColor);
		}
		
		public void StartInteraction(InteractionType type, Action<InteractionContainer, InteractionType> onPressedCallback)
		{
			_type = type;
			_onPressedCallback = onPressedCallback;
			
			if (interactionRenderer) interactionRenderer.enabled = false;
			
			if (pressInteraction) pressInteraction.gameObject.SetActive(false);
			if (holdInteraction) holdInteraction.gameObject.SetActive(false);
			if (compressionInteraction) compressionInteraction.gameObject.SetActive(false);
			
			switch (_type)
			{
				case InteractionType.Press:
					activeInteraction = pressInteraction.gameObject;
					break;
				case InteractionType.Hold:
					activeInteraction = holdInteraction.gameObject;
					break;
				case InteractionType.Compress:
					activeInteraction = compressionInteraction.gameObject;
					break;
			}
			
			if (activeInteraction) activeInteraction.SetActive(true);
		}

		public void InteractionPressed()
		{
			if (interactionRenderer) interactionRenderer.enabled = true;
			if (activeInteraction) activeInteraction.SetActive(false);
			activeInteraction = null;
			_onPressedCallback?.Invoke(this, _type);
		}
	}
}

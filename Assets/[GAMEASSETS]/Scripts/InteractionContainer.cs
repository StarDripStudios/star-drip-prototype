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

		public int Index { get; private set; }
		
		private InteractionType _type;
		private Action<InteractionContainer, InteractionType> _onPressedCallback;
		private Interaction activeInteraction;

		public void Indicate(int index, Color color)
		{
			if (!interactionRenderer ||
			    !interactionRenderer.material) return;

			Index = index;
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
					activeInteraction = pressInteraction;
					break;
				case InteractionType.Hold:
					activeInteraction = holdInteraction;
					break;
				case InteractionType.Compress:
					activeInteraction = compressionInteraction;
					break;
			}
			
			if (activeInteraction) activeInteraction.gameObject.SetActive(true);
		}

		public void InteractionPressed()
		{
			ResetInteraction();
			_onPressedCallback?.Invoke(this, _type);
		}

		public void ResetInteraction()
		{
			if (interactionRenderer) interactionRenderer.enabled = true;
			if (!activeInteraction) return;
		
			activeInteraction.ResetInteraction();
			activeInteraction.gameObject.SetActive(false);
			activeInteraction = null;
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
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
		
		public void StartInteraction(InteractionType type, Action onPressedCallback)
		{
			if (interactionRenderer) interactionRenderer.enabled = false;
			
			if (pressInteraction) pressInteraction.gameObject.SetActive(false);
			if (holdInteraction) holdInteraction.gameObject.SetActive(false);
			if (compressionInteraction) compressionInteraction.gameObject.SetActive(false);
			
			switch (type)
			{
				case InteractionType.Press:
					if (pressInteraction) pressInteraction.gameObject.SetActive(true);
					break;
				case InteractionType.Hold:
					if (holdInteraction) holdInteraction.gameObject.SetActive(true);
					break;
				case InteractionType.Compress:
					if (compressionInteraction) compressionInteraction.gameObject.SetActive(true);
					break;
			}
		}
	}
}

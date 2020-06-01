using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarDust
{
	public class Level : MonoBehaviour
	{
		public Image indicationImage;
		public Color defaultColor;
		public float timeRemaining;

		private Dictionary<InteractionContainer, InteractionType> _sequenceLookup;

		private InteractionContainer _currentInteraction;

		public void PrepareSequence()
		{
			_sequenceLookup?.Clear();
			_sequenceLookup = new Dictionary<InteractionContainer, InteractionType>();
		}
		
		public void Indicate(InteractionContainer interaction, InteractionType type)
		{
			if (!indicationImage) return;

			_currentInteraction = interaction;

			var indicationColor = Color.white;
			switch (type)
			{
				case InteractionType.Press:
					indicationColor = Color.blue;
					break;
				case InteractionType.Hold:
					indicationColor = Color.red;
					break;
				case InteractionType.Compress:
					indicationColor = Color.yellow;
					break;
			}
			
			indicationImage.color = indicationColor;
			_currentInteraction.Indicate(indicationColor);
			
			_sequenceLookup.Add(_currentInteraction, type);
		}

		public void ResetIndication()
		{
			if (!indicationImage) return;

			indicationImage.color = defaultColor;
			if (_currentInteraction) _currentInteraction.ResetIndication();
			_currentInteraction = null;
		}

		public void StartWaitingForInput()
		{
			foreach (var sequence in _sequenceLookup)
			{
				var interaction = sequence.Key;
				if (!interaction) continue;
				interaction.StartInteraction(sequence.Value, ReportInteraction);
			}
		}

		public void Tick(float time)
		{
			timeRemaining = time;
		}

		public void EvaluateSequence()
		{
			
		}

		private void ReportInteraction(InteractionContainer interaction)
		{
			if (!interaction) return;
			
			
		}
	}
}

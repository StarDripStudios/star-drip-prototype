using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace StarDust
{
	public class Level : MonoBehaviour
	{
		public PlayableDirector director;
		public GameObject startPointsContainer;
		public GameObject interactionContainer;
		public Image indicationImage;
		public Color defaultColor;
		public float timeRemaining;
		
		private Dictionary<InteractionContainer, InteractionType> _sequenceLookup;
		private Dictionary<InteractionContainer, InteractionType> _enteredInteractions = new Dictionary<InteractionContainer, InteractionType>();
		private InteractionContainer _currentInteraction;

		public void StartLevel()
		{
			if (startPointsContainer) startPointsContainer.SetActive(false);
			if (interactionContainer) interactionContainer.SetActive(true);
			if (director) director.Play();			
		}
		
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
			_enteredInteractions?.Clear();

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

		private void ReportInteraction(InteractionContainer interaction, InteractionType type)
		{
			if (!interaction) return;
			
			_enteredInteractions.Add(interaction, type);

			var typeComparer = EqualityComparer<InteractionContainer>.Default;
			var valueComparer = EqualityComparer<InteractionType>.Default;

			//Compare keys and values of entered dict to sequence dict
			foreach (var entered in _enteredInteractions)
			{
				//if can't get key from entered out of sequence, it's a miss
				//if the values don't match, it's a miss
				if (!_sequenceLookup.TryGetValue(entered.Key, out type))
				{
					Miss();
					return;
				}

				if (valueComparer.Equals(entered.Value, type)) continue;
				
				Miss();
				return;
			}
			
			//if we make it here, it's not
			
			//Sequence is a success, evaluate
			if (_enteredInteractions == _sequenceLookup) EvaluateSequence();
		}

		private void Miss()
		{
			
		}
		
		public void EvaluateSequence()
		{
			
		}
	}
}

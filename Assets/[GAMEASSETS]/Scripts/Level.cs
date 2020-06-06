using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace StarDust
{
	public class Level : MonoBehaviour
	{
		public PlayableDirector director;
		public PlayableAsset[] sequences;
		public GameObject startPointsContainer;
		public GameObject interactionContainer;
		public Image indicationImage;
		public TextMeshProUGUI statusIndicator;
		public Color defaultColor;
		public float timeRemaining;

		private int _sequenceIndex;
		private Dictionary<InteractionContainer, InteractionType> _sequenceLookup;
		private Dictionary<InteractionContainer, InteractionType> _enteredInteractions = new Dictionary<InteractionContainer, InteractionType>();
		private InteractionContainer _currentInteraction;

		public void StartLevel()
		{
			if (startPointsContainer) startPointsContainer.SetActive(false);
			if (interactionContainer) interactionContainer.SetActive(true);
			PlayNextSequence();
		}
		
		private void PlayNextSequence()
		{
			if (!director) return;
			
			director.Stop();
			
			//Level is done
			if (sequences.Length <= _sequenceIndex)
			{
				EvaluateLevel();
				return;
			}
			
			director.playableAsset = sequences[_sequenceIndex];
			director.Play();
			
			_sequenceIndex++;
		}

		private void ReportInteraction(InteractionContainer interaction, InteractionType type)
		{
			if (!interaction) return;

			//Id the count of entered interactions don't match the current interaction index, it's a miss
			if (_enteredInteractions.Count != interaction.Index)
			{
				EvaluateSequence(false);
				return;
			}
			
			_enteredInteractions.Add(interaction, type);

			//Compare keys and values of entered dict to sequence dict
			var valueComparer = EqualityComparer<InteractionType>.Default;
			foreach (var entered in _enteredInteractions)
			{
				//if can't get key from entered out of sequence, it's a miss
				if (!_sequenceLookup.TryGetValue(entered.Key, out type))
				{
					EvaluateSequence(false);
					break;
				}

				//if the values don't match, it's a miss
				if (valueComparer.Equals(entered.Value, type)) continue;
				EvaluateSequence(false);
				return;
			}
			
			//Sequence is a success, evaluate
			if (_enteredInteractions.Count != _sequenceLookup.Count) return;
			EvaluateSequence(true);
		}

		public void EvaluateSequence(bool isSuccess)
		{
			StartCoroutine(StatusRoutine(isSuccess ? "Sequence Success" : "Sequence Fail"));
			PlayNextSequence();
		}
		
		private void EvaluateLevel()
		{
			StartCoroutine(StatusRoutine("Complete"));
		}

		private IEnumerator StatusRoutine(string message)
		{
			if(!statusIndicator) yield break;

			statusIndicator.text = message;
			yield return new WaitForSeconds(1);
			statusIndicator.text = string.Empty;
		}
		
		#region Timeline Calls
		
		public void PrepareSequence()
		{
			if (_sequenceLookup != null)
			{
				foreach (var interaction in _sequenceLookup)
				{
					if (!interaction.Key) continue;
					interaction.Key.ResetInteraction();
				}

				_sequenceLookup.Clear();
			}

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
			_currentInteraction.Indicate(_sequenceLookup.Count, indicationColor);
			
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
		
		#endregion Timeline Calls

	}
}

using System;
using Framework.Playables;
using UnityEngine;
using UnityEngine.Playables;

namespace StarDust
{
	[Serializable]
	public class IndicationBehaviour : PlayableBehaviourBase
	{
		public InteractionType interactionType;

		public InteractionContainer interaction { private get; set; }
		
		public override void OnEnter(Playable playable, int i)
		{
			var indication = (Level)AttachObject;
			if (!indication || !interaction) return;
			
			Debug.Log("Showing indication");

			indication.Indicate(interaction, interactionType);
			
			base.OnEnter(playable, i);
		}

		public override void OnStay(Playable playable, int i, float time)
		{
			base.OnStay(playable, i, time);
		}

		public override void OnExit(Playable playable, int i)
		{
			var indication = (Level)AttachObject;
			if (!indication) return;
			
			Debug.Log("Disabling indication");
			
			indication.ResetIndication();
			base.OnExit(playable, i);
		}
	}
}

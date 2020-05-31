using System;
using Framework.Playables;
using Ixion.Prototype;
using UnityEngine;
using UnityEngine.Playables;

namespace StarDust
{
	[Serializable]
	public class IndicationBehaviour : PlayableBehaviourBase
	{
		public InteractionType interactionType;
		
		public override void OnEnter(Playable playable, int i)
		{
			var indication = (Indication)AttachObject;
			if (!indication) return;
			
			Debug.Log("Showing indication");

			switch (interactionType)
			{
				case InteractionType.Press:
					indication.Indicate(Color.blue);
					break;
				case InteractionType.Hold:
					indication.Indicate(Color.red);
					break;
				case InteractionType.Compress:
					indication.Indicate(Color.yellow);
					break;
			}
			
			base.OnEnter(playable, i);
		}

		public override void OnExit(Playable playable, int i)
		{
			var indication = (Indication)AttachObject;
			if (!indication) return;
			
			Debug.Log("Disabling indication");
			
			indication.ResetIndication();
			base.OnExit(playable, i);
		}
	}
	
}

using System;
using Framework.Playables;
using Ixion.Prototype;
using UnityEngine;
using UnityEngine.Playables;

namespace StarDust
{
	[Serializable]
	public class InteractionBehaviour : PlayableBehaviourBase
	{
		public override void OnEnter(Playable playable, int i)
		{
			base.OnEnter(playable, i);
		}

		public override void OnExit(Playable playable, int i)
		{
			base.OnExit(playable, i);
		}
	}
}

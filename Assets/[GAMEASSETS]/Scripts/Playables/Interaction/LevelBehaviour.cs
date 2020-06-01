using System;
using Framework.Playables;
using UnityEngine.Playables;

namespace StarDust
{
	[Serializable]
	public class LevelBehaviour : PlayableBehaviourBase
	{
		public LevelState state;
		
		public override void OnEnter(Playable playable, int i)
		{
			var level = (Level)AttachObject;
			if(!level) return;

			switch (state)
			{
				case LevelState.Prepare:
					level.PrepareSequence();
					break;
				case LevelState.Process:
					level.StartWaitingForInput();
					break;
			}

			base.OnEnter(playable, i);
		}

		public override void OnStay(Playable playable, int i, float time)
		{
			var level = (Level)AttachObject;
			if(!level) return;
			
			if (state != LevelState.Process) return;
			
			level.Tick(time / (float)OwnedClip.end);
			
			base.OnStay(playable, i, time);
		}

		public override void OnExit(Playable playable, int i)
		{
			var level = (Level)AttachObject;
			if(!level) return;
			
			if (state != LevelState.Process) return;
			
			level.EvaluateSequence();
			
			base.OnExit(playable, i);
		}

		public enum LevelState
		{
			Prepare,
			Process
		}
	}
}

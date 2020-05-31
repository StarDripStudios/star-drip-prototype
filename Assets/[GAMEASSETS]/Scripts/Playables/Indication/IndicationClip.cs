using Framework.Playables;
using UnityEngine;
using UnityEngine.Playables;

namespace StarDust
{
	public class IndicationClip : PlayableClipBase<IndicationBehaviour>
	{
		public ExposedReference<InteractionContainer> interaction;
		
		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			var playable = ScriptPlayable<IndicationBehaviour>.Create(graph, template);
			var clone = playable.GetBehaviour();

			clone.AttachObject = AttachObject;
			clone.OwnedClip = OwnedClip;
			clone.interaction = interaction.Resolve(graph.GetResolver());

			return playable;
		}
	}
}

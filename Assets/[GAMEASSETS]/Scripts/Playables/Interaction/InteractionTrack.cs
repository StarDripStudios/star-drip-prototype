using System.Collections;
using System.Collections.Generic;
using Framework.Playables;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace StarDust
{
	[TrackColor(0.5f,0.5f,0)]
	[TrackClipType(typeof(InteractionClip))]
	public class InteractionTrack : PlayableTrackBase<InteractionBehaviour, InteractionMixerBehaviour, InteractionClip>
	{
		
	}
}

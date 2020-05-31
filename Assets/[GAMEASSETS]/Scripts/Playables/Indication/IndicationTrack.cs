using System.Collections;
using System.Collections.Generic;
using Framework.Playables;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace StarDust
{
	[TrackColor(0,0,0)]
	[TrackClipType(typeof(IndicationClip))]
	[TrackBindingType(typeof(Indication))]
	public class IndicationTrack : PlayableTrackBase<IndicationBehaviour, IndicationMixerBehaviour, IndicationClip>
	{
		
	}
}

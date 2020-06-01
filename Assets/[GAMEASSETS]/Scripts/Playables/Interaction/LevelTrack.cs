using Framework.Playables;
using UnityEngine.Timeline;

namespace StarDust
{
	[TrackColor(0.5f,0.5f,0)]
	[TrackClipType(typeof(LevelClip))]
	[TrackBindingType(typeof(Level))]
	public class LevelTrack : PlayableTrackBase<LevelBehaviour, LevelMixerBehaviour, LevelClip>
	{
		
	}
}

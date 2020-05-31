using UnityEngine;
using UnityEngine.UI;

namespace StarDust
{
	public class Level : MonoBehaviour
	{
		public Image indicationImage;
		public Color defaultColor;
		
		public void Indicate(Color color)
		{
			if (!indicationImage) return;

			indicationImage.color = color;
		}

		public void ResetIndication()
		{
			if (!indicationImage) return;

			indicationImage.color = defaultColor;
		}
	}
}

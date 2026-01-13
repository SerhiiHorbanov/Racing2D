using UnityEngine;

namespace Data
{
	public class RacerConfiguration
	{
		private Color _color = Color.white;
		public Color Color
		{
			get => _color;
			set => _color = new(value.r, value.g, value.b, 1);
		}
	}
}

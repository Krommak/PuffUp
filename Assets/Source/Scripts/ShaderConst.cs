using UnityEngine;

namespace Zlodey
{
	public static class ShaderConst
	{
		public static readonly int Outline = Shader.PropertyToID("_OutlineColor");
		public static readonly int Color = Shader.PropertyToID("_Color");
		public static readonly int ShadowColor = Shader.PropertyToID("_SColor");
	}
}
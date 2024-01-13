using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blooper.TransitionEffects
{
	[Serializable]
	public class OutlineEffectPassSettings
	{
		[HideInInspector] public FilterMode FilterMode = FilterMode.Bilinear; //In my testing, this doesn't have an effect. Because we are doing it at screen-resolution, by definition, there's no sampling anyway.
		
		public bool Active = true;
		[FormerlySerializedAs("transition")] [Tooltip("0 has The scene completely visible. 1 Has the transition effect completely obscuring the scene.")] [Range(0, 1)]
		public float Threshold;
		public Color Color; //Luckily, black is already a pretty good default value :p
		public string GetShaderName()
		{
			return "Hidden/BloopEdgeDetectionShader";
		}

		public void CopyFrom(OutlineEffectPassSettings settings)
		{
			this.Active = settings.Active;
			this.Threshold = settings.Threshold;
			this.Color = settings.Color;
		}
	}
}
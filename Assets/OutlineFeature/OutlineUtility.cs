using System.Collections.Generic;
using System.Reflection;
using Blooper.TransitionEffects;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace OutlineFeature
{
	public static class OutlineUtility
	{
		public static OutlineEffectRenderFeature FindTransitionEffectRenderFeature()
		{
			var renderer = ((UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline).GetRenderer(0);

			var property =
				typeof(ScriptableRenderer).GetProperty("rendererFeatures",
					BindingFlags.NonPublic | BindingFlags.Instance);
			List<ScriptableRendererFeature> features = property.GetValue(renderer) as List<ScriptableRendererFeature>;
			foreach (var feature in features)
			{
				if (feature is OutlineEffectRenderFeature transitionFeature)
				{
					var transitionEffect = transitionFeature;
					return transitionEffect;
				}
			}

			//if null...
			Debug.LogWarning("You need to add an Outline Render Feature to the current render pipeline settings.");
			return null;
		}
	}
}
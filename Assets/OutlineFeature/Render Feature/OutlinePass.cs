using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Blooper.TransitionEffects
{
	public class OutlinePass : ScriptableRenderPass
	{
		private readonly OutlineEffectPassSettings _settings;
		private Material _material;

		//Cache properties
		private int _bufferID = Shader.PropertyToID("_OutlineBuffer");
		private static readonly int ColorPropID = Shader.PropertyToID("_EdgeColor");
		private static readonly int ThresholdID = Shader.PropertyToID("_Threshold");

		private RenderTargetIdentifier _bufferRenderTex;
		private TransitionType _currentType;
		private static readonly int TransitionTexturePropID = Shader.PropertyToID("_BufferTexture");

		public OutlinePass(OutlineEffectPassSettings settings, OutlinePass _clone)
		{
			_settings = settings;
			renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
			_material = CoreUtils.CreateEngineMaterial(_settings.GetShaderName());
			// ...
		}

		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			base.Configure(cmd, cameraTextureDescriptor);
			
			cmd.GetTemporaryRT(_bufferID, cameraTextureDescriptor, _settings.FilterMode);
			
			//todo: RTHandle
			_bufferRenderTex = new RenderTargetIdentifier(_bufferID);
			// ...
		}

		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			base.OnCameraSetup(cmd, ref renderingData);
			// ...
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			//only render on top of the game. (ie: not reflections, scene view, VR, editor preview windows)
			if (renderingData.cameraData.cameraType != CameraType.Game || !_settings.Active)
			{
				return;
			}

			var target = renderingData.cameraData.renderer.cameraColorTargetHandle;

			CommandBuffer cmd = CommandBufferPool.Get();
			cmd.Clear();
			_material.SetColor(ColorPropID, _settings.Color);
			_material.SetFloat(ThresholdID, _settings.Threshold);
			Blit(cmd, target, _bufferRenderTex, _material, 0);
			Blit(cmd, _bufferRenderTex, target);
			context.ExecuteCommandBuffer(cmd);

			cmd.Clear();
			CommandBufferPool.Release(cmd);
		}

		public override void OnCameraCleanup(CommandBuffer cmd)
		{
			base.OnCameraCleanup(cmd);
			cmd.ReleaseTemporaryRT(_bufferID);
		}
	}
}
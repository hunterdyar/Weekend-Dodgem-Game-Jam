using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace Blooper.TransitionEffects
{
    public class OutlineEffectRenderFeature : ScriptableRendererFeature
    {
        [SerializeField] private OutlineEffectPassSettings _settings = new();
        private OutlinePass _pass;

        public override void Create()
        {
            _pass = new OutlinePass(_settings, _pass);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_pass);
        }
        public void SetThreshold(float transitionLerp)
        {
            _settings.Threshold = Mathf.Clamp01(transitionLerp);
        }
        
        public void SetColor(Color color)
        {
            _settings.Color = color;
        }

        /// <summary>
        /// The Transition will use it's internal settings class, but copy the properties from the provided setting without referencing it.
        /// </summary>
        public void CopySettingsFrom(OutlineEffectPassSettings settings)
        {
            _settings.CopyFrom(settings);
        }

        /// <summary>
        /// The Transition will use the provided settings class for it's settings.
        /// </summary>
        public void SetSettings(OutlineEffectPassSettings settings)
        {
            _settings = settings;
        }
    }
}
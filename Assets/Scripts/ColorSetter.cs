using OutlineFeature;
using TMPro;
using UnityEngine;


public class ColorSetter : MonoBehaviour
{
	public Color Background;
	public Color Foreground;

	void Awake()
	{
		var visuals = OutlineUtility.FindTransitionEffectRenderFeature();
		visuals.SetColor(Foreground);

		var texts = GameObject.FindObjectsOfType<TMP_Text>();
		foreach (var text in texts)
		{
			text.color = Background;
			text.outlineColor = (Color32)Foreground;
		}
	}
}

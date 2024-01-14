
using UnityEngine;

[System.Serializable]
public class WeightedItem<T>
{
	public T item;
	[Min(0)]
	public float weight;
	public AnimationCurve Curve;

	public void SetWeightByCurve(float x)
	{
		weight = Curve.Evaluate(x);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

[System.Serializable]
public class WeightedSelection<T>
{
	public List<WeightedItem<T>> Items = new List<WeightedItem<T>>();
	public int Count => Items.Count;
	
	public WeightedItem<T> GetWeightedItem(T key)
	{
		return Items.Find(x => x.item.Equals(key));
	}

	public bool TryGetWeightedItem(T key, out WeightedItem<T> item)
	{
		item = Items.Find(x => x.item.Equals(key));
		if (item != null)
		{
			return true;
		}

		return false;
	}

	public void SetAllItemWeightsByCurve(float x)
	{
		foreach (var item in Items)
		{
			item.SetWeightByCurve(x);
		}
	}

	public WeightedItem<T> GetWeightedItemByIndex(int index)
	{
		return Items[index];
	}
	public T GetWeightedRandomItem()
	{
		if (Items.Count == 0)
		{
			throw new IndexOutOfRangeException("Can't get random item from empty selection.");
			return default;
		}
		float total = Items.Sum(x => x.weight);
		float random = UnityEngine.Random.Range(0, total);

		float running = 0;
		for (int i = 0; i < Items.Count; i++)
		{
			running += Items[i].weight;
			if (random <= running)
			{
				return Items[i].item;
			}
		}

		throw new IndexOutOfRangeException("Unable to select random item. Are all weights valid values?");
	}
}

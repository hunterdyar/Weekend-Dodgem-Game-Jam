
using UnityEngine;

public static class Extensions
{
	public static int ToSign(this bool b)
	{
		return b ? 1 : -1;
	}

	public static Vector3 RandomPointInBox(this BoxCollider2D box)
	{
		return new Vector3(Random.Range(box.bounds.min.x, box.bounds.max.x),
			Random.Range(box.bounds.min.y, box.bounds.max.y), 0);
	}
}

using UnityEngine;
using DefaultNamespace;

public class Trap : Interactable
{
	public float rotationSpeed;

	public override void Interact(PlayerMovement player)
	{
		base.Interact(player);
		player.Trap(this);
		Destroy(gameObject);
	}

	public Quaternion GetRotation()
	{
		return Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
	}
}

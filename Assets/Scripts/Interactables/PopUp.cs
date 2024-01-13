using UnityEngine;

namespace DefaultNamespace
{
	public class PopUp : Interactable
	{
		public override void Interact(PlayerMovement player)
		{
			base.Interact(player);
			player.Rigidbody.velocity += Vector2.up * player.jumpSpeed * 0.5f;
			Destroy(gameObject);
		}
	}
}
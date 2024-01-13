using UnityEngine;

namespace DefaultNamespace
{
	public class Death : Interactable
	{
		public override void Interact(PlayerMovement player)
		{
			base.Interact(player);
			player.Die();
		}
	}
}
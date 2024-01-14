using System;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
	public class PlayerInput : MonoBehaviour
	{
		private PlayerMovement _movement;

		private void Awake()
		{
			_movement = GetComponent<PlayerMovement>();
		}

		public void Update()
		{
		//	_movement.Aim(new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")));
			if (Input.GetButton("Jump"))
			{
				_movement.Jump();
			}

			if (Input.GetKeyDown(KeyCode.S))
			{
				_movement.Manager.ToggleScreenShake();
			}
		}
	}
}
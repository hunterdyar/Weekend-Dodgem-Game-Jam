using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
	public class PlayerMovement : MonoBehaviour
	{
		public GameplayManager Manager;
		public Rigidbody2D Rigidbody => _rb;
		private Rigidbody2D _rb;
		private PlayerVisuals _pv;
		
		private Vector2 _aim;
		public float jumpSpeed;

		public PlayerState PlayerState => _playerState;
		private PlayerState _playerState;

		public bool Connected => _connectedJoint != null;
		private Rigidbody2D _connectedJoint;
		private Vector2 _previousJointPosition;

		private Vector2 _prevVel;
		

		
		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
			_pv = GetComponent<PlayerVisuals>();
			_aim = Vector2.right;
			_playerState = PlayerState.Inactive;
		}

		private void FixedUpdate()
		{
			if (PlayerState == PlayerState.Dead)
			{
				return;
			}
			
			_prevVel = _rb.velocity;
			if (_connectedJoint != null)
			{
				var delta = _connectedJoint.position - _previousJointPosition;
				_rb.MovePosition(_rb.position+Vector2.down*(Manager.GameplaySpeed*Time.fixedDeltaTime));
				
				//for next frame...
				_previousJointPosition = _connectedJoint.position;
			}
		}

		private void Update()
		{
			if (_playerState == PlayerState.Dead)
			{
				return;
			}

#if UNITY_EDITOR
if (Input.GetKeyDown(KeyCode.Y))
{
	Die();
}
#endif
		}

		public void Jump()
		{
			if (_playerState == PlayerState.Dead)
			{
				return;
			}

			if (_playerState == PlayerState.Inactive)
			{
				//start the game, then jump (below)
				Manager.ChangeGameState(GameState.Gameplay);
			}

			if (_playerState == PlayerState.Flying || _playerState == PlayerState.Dead)
			{
				return;
			}

			_connectedJoint = null;
			_rb.velocity = _aim.normalized*jumpSpeed;
			_playerState = PlayerState.Flying;

		}

		private void ConnectJoint(Rigidbody2D rb)
		{
			if (Connected)
			{
				Debug.Log("uh oh. connecting twice? hit joint of two objects?");
				return;
			}
			_connectedJoint = rb;
			_previousJointPosition = rb.position;
			_playerState = PlayerState.WallCling;
		}
		private void OnCollisionEnter2D(Collision2D other)
		{
			_rb.velocity = Vector3.zero;
			_aim = other.GetContact(0).normal;//this might not always be right or left. round?
			_aim = new Vector2(_aim.x, 0).normalized;
			var rb = other.rigidbody;
			if (rb != null)
			{
				ConnectJoint(other.rigidbody);
				if (Manager.screenshake)
				{
					CameraShake.ShakeOnce(_aim);
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			var inter = other.GetComponentInParent<Interactable>();
			if (inter != null)
			{
				inter.Interact(this);
			}
		}

		public void Die()
		{
			_rb.velocity = Vector2.zero;
			//Explode
			_playerState = PlayerState.Dead;
			_rb.isKinematic = true;
			GetComponentInChildren<Collider2D>().enabled = false;
			_pv.Explode();
			Manager.PlayerDied(this);
		}

		
	}
}
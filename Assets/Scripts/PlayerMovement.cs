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
		private FixedJoint2D _joint;

		private Vector2 _aim;
		public float jumpSpeed;

		//todo: replace with gameplaymanager state check
		public bool firstJumpTaken = false;
		public bool Connected => _connectedJoint != null;
		private Rigidbody2D _connectedJoint;
		private Vector2 _previousJointPosition;

		private Vector2 _prevVel;
		private bool movementActive = true;
		[Header("Death Settings")] public GameObject DeathNugget;
		public int disectionCount;
		public float explosionForce;
		private void Awake()
		{
			firstJumpTaken = false;
			_rb = GetComponent<Rigidbody2D>();
			_aim = Vector2.right;
			movementActive = true;
		}

		private void FixedUpdate()
		{
			if (!movementActive)
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
			if (!movementActive)
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
			if (!movementActive)
			{
				return;
			}

			if (!firstJumpTaken)
			{
				//start the game.
				Manager.ChangeGameState(GameState.Gameplay);
			}
			
			if (!Connected && firstJumpTaken)
			{
				return;
			}

			firstJumpTaken = true;
			ReleaseJoint();
			_rb.velocity = _aim.normalized*jumpSpeed;
		}

		private void ReleaseJoint()
		{ 
			_connectedJoint = null;
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
			movementActive = false;
			_rb.isKinematic = true;
			GetComponentInChildren<Collider2D>().enabled = false;
			GetComponentInChildren<SpriteRenderer>().enabled = false;
			ExplodeParticles();
			Manager.PlayerDied();
		}

		private void ExplodeParticles()
		{
			List<Rigidbody2D> nuggets = new List<Rigidbody2D>();
			Vector3 scale = transform.localScale / disectionCount;
			for (int i = 0; i < disectionCount; i++)
			{
				for (int j = 0; j < disectionCount; j++)
				{
					var n = Instantiate(DeathNugget);
					nuggets.Add(n.GetComponent<Rigidbody2D>());
					Vector3 offset = new Vector2(scale.x * i + scale.x/2 - transform.localScale.x/2, scale.y * j+scale.y/2-transform.localScale.y/2);
					n.transform.localScale = scale;
					n.transform.position = transform.position + offset;
				}
			}

			foreach (var n in nuggets)
			{
				n.velocity = _rb.velocity;
				n.AddForce(Random.insideUnitCircle*Random.insideUnitCircle * explosionForce,ForceMode2D.Impulse);
			}
		}
	}
}
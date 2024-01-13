using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class Interactable : MonoBehaviour
	{
		public bool solidOnGameOver;
		private Collider2D _collider2D;

		private void Awake()
		{
			_collider2D = GetComponentInChildren<Collider2D>();
			_collider2D.isTrigger = true;
		}

		private void OnEnable()
		{
			GameplayManager.OnGameStateChange += OnGameStateChange;
		}

		private void OnDisable()
		{
			GameplayManager.OnGameStateChange -= OnGameStateChange;
		}

		private void OnGameStateChange(GameState state)
		{
			if (solidOnGameOver)
			{
				_collider2D.isTrigger = state != GameState.GameOver;
			}
		}

		public virtual void Interact(PlayerMovement player)
		{
		}
	}
}
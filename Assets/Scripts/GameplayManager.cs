using System;
using UnityEngine;

namespace DefaultNamespace
{
	[CreateAssetMenu(fileName = "Gameplay Manager", menuName = "Dodgem/Gameplay", order = 0)]
	public class GameplayManager : ScriptableObject
	{
		public float GameplaySpeed => _gameplaySpeed;
		[SerializeField] private float _gameplaySpeed;
		[Header("Config")] public float StartingSpeed;
		[Range(0,1f)]
		public float speedIncreaseDeltaModifier;
		public static Action<GameState> OnGameStateChange;
		public GameState GameState => _gameState;
		private GameState _gameState;
		
		
		public void ChangeGameState(GameState newGameState)
		{
			if (_gameState == newGameState)
			{
				Debug.LogWarning("Change state to same state. "+newGameState);
				return;
			}

			_gameState = newGameState;
			OnGameStateChange.Invoke(_gameState);
		}

		public void Init()
		{
			_gameState = GameState.Init;
			_gameplaySpeed = StartingSpeed;
			Physics2D.gravity = Vector2.zero;
		}

		public void PlayerDied()
		{
			Physics2D.gravity = new Vector2(0, -GameplaySpeed);
			_gameplaySpeed = 0;
			ChangeGameState(GameState.GameOver);
			Debug.Log("LOSE");
		}

		//called by update. can use time.deltaTime here.
		public void Tick()
		{
			if (_gameState != GameState.Gameplay)
			{
				return;
			}
			_gameplaySpeed += Time.deltaTime * speedIncreaseDeltaModifier;
		}
	}
}
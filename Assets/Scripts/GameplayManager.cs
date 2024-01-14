using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
	[CreateAssetMenu(fileName = "Gameplay Manager", menuName = "Dodgem/Gameplay", order = 0)]
	public class GameplayManager : ScriptableObject
	{
		private static readonly string HighScoreKey = "BestSurvivalTime";
		public float GameplaySpeed => _gameplaySpeed;
		[SerializeField] private float _gameplaySpeed;
		[Header("Config")] public float StartingSpeed;
		[Range(0,1f)]
		public float speedIncreaseDeltaModifier;
		public static Action<GameState> OnGameStateChange;
		public GameState GameState => _gameState;
		private GameState _gameState;

		public float SurvivalTime => _survivalTime;
		private float _survivalTime;
		public bool IsHighscore => _isHighScore;
		private bool _isHighScore = false;
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
			_survivalTime = 0;
			_isHighScore = false;
		}

		public void PlayerDied(PlayerMovement player)
		{
			Physics2D.gravity = new Vector2(0, -GameplaySpeed);
			_gameplaySpeed = 0;
			ChangeGameState(GameState.GameOver);
			Debug.Log("LOSE");
			ScoreHighScore();
			player.StartCoroutine(WaitThenRestart());
		}

		private void ScoreHighScore()
		{
			float highest = PlayerPrefs.GetFloat(HighScoreKey, 0);
			if (_survivalTime > highest)
			{
				PlayerPrefs.SetFloat(HighScoreKey,_survivalTime);
				_isHighScore = true;
			}
		}

		//called by update. can use time.deltaTime here.
		public void Tick()
		{
			if (_gameState != GameState.Gameplay)
			{
				return;
			}

			_survivalTime += Time.deltaTime;
			_gameplaySpeed += Time.deltaTime * speedIncreaseDeltaModifier;
		}

		IEnumerator WaitThenRestart()
		{
			//wait for the gameOver text and the circle transitions to be finished.
			//but for now, we'll hard code it.
			yield return new WaitForSeconds(1.45f);
			RestartGame();
		}
		
		public void RestartGame()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
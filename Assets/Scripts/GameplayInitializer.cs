using System;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
	public class GameplayInitializer : MonoBehaviour
	{
		public GameplayManager Manager;

		private void Awake()
		{
			Manager.Init();
		}

		private void Update()
		{
			Manager.Tick();
		}
	}
}
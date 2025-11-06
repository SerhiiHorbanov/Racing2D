using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Lobby
{
	public class Lobby : MonoBehaviour
	{
		[SerializeField] private int _MaxRacers;
		[SerializeField] private GameObject _AIRacerPrefab;
		
		[SerializeField] private RacersList _Racers;

		private PlayerInputManager _playerInputManager;

		[SerializeField] private string _GameScene;

		private int _aiRacers;
		private int _playerRacers;
		
		private void Awake()
		{
			_playerInputManager = GetComponent<PlayerInputManager>();
		}

		public void StartGame()
		{
			SceneManager.LoadScene(_GameScene);
		}

		public void OnPlayerJoined(PlayerInput input)
		{
			_Racers.Add(input.GetComponent<IRacer>());
			_playerRacers++;
			UpdatePlayersLimit();
		}
		
		public void OnPlayerLeft(PlayerInput input)
		{
			_Racers.Remove(input.GetComponent<IRacer>());
			_playerRacers--;
			UpdatePlayersLimit();
		}
		
		public void AddAIRacer()
		{
			if (_MaxRacers <= _aiRacers + _playerRacers)
			{
				Debug.LogWarning("Not adding AI racer because racers limit reached");
				return;
			}
			
			GameObject o = Instantiate(_AIRacerPrefab);
			_Racers.Add(o.GetComponent<IRacer>());
			_aiRacers++;
			UpdatePlayersLimit();
		}

		public void RemoveAIRacer(AIRacer racer)
		{
			if (_Racers.Remove(racer));
			{
				_aiRacers--;
				UpdatePlayersLimit();
			}
		}

		private void UpdatePlayersLimit()
		{
			if (_MaxRacers <= _aiRacers + _playerRacers)
			{
				_playerInputManager.DisableJoining();
			}
			else
			{
				_playerInputManager.EnableJoining();
			}
		}

		public void RemoveAllRacers()
		{
			_aiRacers = 0;
			_playerRacers = 0;
			UpdatePlayersLimit();
			for (int i = 0; i < 100; i++)
			{
				if (_Racers.Racers.Count == 0)
					return;
				
				MonoBehaviour component = _Racers.Racers[0] as MonoBehaviour;
				Destroy(component?.gameObject);
			}

			Debug.LogError("Failed to remove all racers");
		}
	}

	[CustomEditor(typeof(Lobby))]
	class DecalMeshHelperEditor : Editor {
		public override void OnInspectorGUI() 
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Add AI racer"))
				(target as Lobby)?.AddAIRacer();
			if (GUILayout.Button("Remove all racers"))
				(target as Lobby)?.RemoveAllRacers();
			if (GUILayout.Button("Start game"))
				(target as Lobby)?.StartGame();
		}
	}
}
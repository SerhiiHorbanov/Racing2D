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
		[SerializeField] private int _MaxPlayers;
		[SerializeField] private GameObject _AIRacerPrefab;
		
		[SerializeField] private RacersList _Racers = new();

		private PlayerInputManager _playerInputManager;

		[SerializeField] private string _GameScene;
		
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
		}
		
		public void OnPlayerLeft(PlayerInput input)
		{
			_Racers.Remove(input.GetComponent<IRacer>());
		}
		
		public void AddAIRacer()
		{
			GameObject o = Instantiate(_AIRacerPrefab);
			_Racers.Add(o.GetComponent<IRacer>());
		}

		public void RemoveAIRacer(AIRacer racer)
		{
			_Racers.Remove(racer);
		}

		public void RemoveAllRacers()
		{
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
using System;
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
		
		public Action<IRacer> OnRacerAdded;
		public Action<IRacer> OnRacerRemoved;
		
		private void Awake()
		{
			_playerInputManager = GetComponent<PlayerInputManager>();
		}

		public void StartGame()
		{
			SceneManager.LoadScene(_GameScene);
		}

		private void InvokeOnRacerAdded(IRacer racer)
		{
			UpdatePlayersLimit();
			OnRacerAdded?.Invoke(racer);
		}

		private void InvokeOnRacerRemoved(IRacer racer)
		{
			UpdatePlayersLimit();
			OnRacerRemoved?.Invoke(racer);
		}
		
		public void OnPlayerJoined(PlayerInput input)
		{
			IRacer racer = input.GetComponent<IRacer>();
			_Racers.Add(racer);
			
			InvokeOnRacerAdded(racer);
		}
		
		public void OnPlayerLeft(PlayerInput input)
		{
			IRacer racer = input.GetComponent<IRacer>();
			if (_Racers.Remove(racer));
			
			InvokeOnRacerRemoved(racer);
		}
		
		public void AddAIRacer()
		{
			if (_Racers.Racers.Count >= _MaxRacers)
			{
				Debug.LogWarning("Not adding AI racer because racers limit reached");
				return;
			}
			
			GameObject o = Instantiate(_AIRacerPrefab);
			IRacer racer = o.GetComponent<IRacer>();
			_Racers.Add(racer);
			InvokeOnRacerAdded(racer);
		}

		public void RemoveRacer(IRacer racer)
		{
			if (!_Racers.Remove(racer))
				return;
			
			MonoBehaviour component = racer as MonoBehaviour;
			Destroy(component?.gameObject);
			
			UpdatePlayersLimit();
			InvokeOnRacerRemoved(racer);
		}

		private void UpdatePlayersLimit()
		{
			if (_Racers.Racers.Count >= _MaxRacers)
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
			UpdatePlayersLimit();
			for (int i = 0; i < 100; i++)
			{
				if (_Racers.Racers.Count == 0)
					return;
				
				RemoveRacer(_Racers.Racers[0]);
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
using Lobby;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class StartGameButton : MonoBehaviour
	{
		[SerializeField] private Lobby.Lobby _lobby;
		[SerializeField] private RacersList _racers;
		private Button _button;
	
		private void Awake()
		{
			_button = GetComponent<Button>();
			if (_lobby != null)
			{
				_lobby.OnRacerAdded += UpdateInteractability;
				_lobby.OnRacerRemoved += UpdateInteractability;
			}
			else
			{
				Debug.LogWarning("Lobby not set in StartGameButton");
			}
		}

		private void OnDestroy()
		{
			if (_lobby != null)
			{
				_lobby.OnRacerAdded -= UpdateInteractability;
				_lobby.OnRacerRemoved -= UpdateInteractability;
			}
		}
		
		private void UpdateInteractability(IRacer _)
		{
			_button.interactable = _racers.Racers.Count > 0;
		}

		public void StartGame()
		{
			_lobby.StartGame();
		}
	}
}

using System.Collections.Generic;
using UnityEngine;
using Lobby;
using UnityEngine.Serialization;

namespace UI
{
	// manages interactions between RacerPanels and Lobby
	public class RacerPanelsManager : MonoBehaviour
	{
		[FormerlySerializedAs("lobby")] [SerializeField] private Lobby.Lobby _Lobby;
		[FormerlySerializedAs("racerPanelPrefab")] [SerializeField] private GameObject _RacerPanelPrefab;
		[SerializeField] private Transform _JoiningActionText;
		private Dictionary<IRacer, RacerPanel> _panels;  
		
		private void Awake()
		{
			_panels = new();
			ConnectToLobby();
		}

		public void AddAIRacer()
		{
			_Lobby?.AddAIRacer();
		}
		
		private void ConnectToLobby()
		{
			if (_Lobby == null)
			{
				Debug.LogError("Lobby is not set in RacerPanelsManager");
				return;
			}

			_Lobby.OnRacerAdded += AddRacerPanel;
			_Lobby.OnRacerRemoved += RemoveRacerPanel;
		}
		
		private void AddRacerPanel(IRacer racer)
		{
			GameObject instantiated = Instantiate(_RacerPanelPrefab, gameObject.transform);
			RacerPanel racerPanel = instantiated.GetComponent<RacerPanel>();
			_panels.Add(racer, racerPanel);
			
			racerPanel.SetRacer(racer);
			racerPanel.RemoveRacer += RemoveRacer;
			
			_JoiningActionText.SetAsLastSibling();
		}

		private void RemoveRacer(IRacer racer)
		{
			RemoveRacerPanel(racer);
			_Lobby.RemoveRacer(racer);
		}
		
		private void RemoveRacerPanel(IRacer racer)
		{
			if (!_panels.TryGetValue(racer, out RacerPanel racerPanel))
				return;

			racerPanel.RemoveRacer -= RemoveRacerPanel;
			Destroy(racerPanel.gameObject);
			_panels.Remove(racer);
		}
	}
}

using Controlling;
using Lobby;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserRacer : MonoBehaviour, IRacer
{
	[SerializeField] private GameObject _ControllerPrefab;
	private PlayerInput _playerInput;
	private RacerScore _score;
	
	public RaceCar Car { get; set; }
	
	private void Awake()
	{
		_playerInput = GetComponent<PlayerInput>();
		_score = GetComponent<RacerScore>();
		DontDestroyOnLoad(gameObject);
	}

	public void SpawnController(RaceCar car)
	{
		GameObject controllerGameObject = Instantiate(_ControllerPrefab);
		PlayerCarController controller = controllerGameObject.GetComponent<PlayerCarController>();
		
		controller.ConnectTo(_playerInput);
		controller._Car = car;
	}

	public RacerScore GetScore()
		=> _score;
}
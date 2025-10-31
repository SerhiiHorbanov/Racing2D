using Controlling;
using Lobby;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserRacer : MonoBehaviour, IRacer
{
	[SerializeField] private GameObject _ControllerPrefab;
	private PlayerInput _playerInput;
	private RacerScore _score = new();
	private PlayerCarController _carController;
	
	public RaceCar Car { get; set; }
	
	private void Awake()
	{
		_playerInput = GetComponent<PlayerInput>();
		DontDestroyOnLoad(gameObject);
	}

	public void InitializeCarController()
	{
		GameObject controllerGameObject = Instantiate(_ControllerPrefab);
		_carController = controllerGameObject.GetComponent<PlayerCarController>();
		_carController.ConnectTo(_playerInput);
	}

	public void ConnectCarControllerTo(RaceCar car)
	{
		if (_carController is null)
			InitializeCarController();
		
		_carController._Car = car;
		_carController.enabled = true;
	}

	public void DisconnectCarControllerFromCar()
	{
		if (_carController is null)
			return;
			
		_carController._Car = null;
		_carController.enabled = false;
	}

	public RacerScore GetScore()
		=> _score;
}
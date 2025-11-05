using Controlling;
using Lobby;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserRacer : MonoBehaviour, IRacer
{
	[SerializeField] private GameObject _ControllerPrefab;
	[SerializeField] private GameObject _CursorControllerPrefab;
	private PlayerInput _playerInput;
	private RacerScore _score = new();
	private PlayerCarController _carController;
	private PlayerCursorController _cursorController;
	
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

	public void EnableCarController(RaceCar car)
	{
		if (_carController == null)
			InitializeCarController();
		
		_carController._Car = car;
		_carController.enabled = true;
	}

	public void DisableCarController()
	{
		if (_carController is null)
			return;
			
		_carController._Car = null;
		_carController.enabled = false;
	}

	public void ConnectRacerCursorControllerTo(RacerCursor cursor)
	{
		if (_cursorController is null)
			InitializeCursorController();
		
		_cursorController.ConnectToCursor(cursor);
		_cursorController.enabled = true;
	}

	public void DisconnectRacerCursorControllerFromCursor()
	{
		_cursorController.DisconnectFromCursor();
	}

	public void InitializeCursorController()
	{
		GameObject cursorControllerGameObject = Instantiate(_CursorControllerPrefab);
		_cursorController = cursorControllerGameObject.GetComponent<PlayerCursorController>();
		_cursorController.ConnectToInput(_playerInput);
	}
	
	public RacerScore GetScore()
		=> _score;
}
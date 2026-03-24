using Controlling;
using Data;
using Lobby;
using UnityEngine;

public class AIRacer : Racer
{
	[SerializeField] private GameObject _ControllerPrefab;
	[SerializeField] private GameObject _CursorControllerPrefab;
	
	private AICarController _carController;
	
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void InitializeCarController()
	{
		GameObject controllerGameObject = Instantiate(_ControllerPrefab);
		_carController = controllerGameObject.GetComponent<AICarController>();
	}

	public override void EnableCarController(RaceCar car)
	{
		if (_carController == null)
			InitializeCarController();
		
		_carController._Car = car;
		_carController.enabled = true;
	}

	public override void DisableCarController()
	{
		if (_carController is null)
			return;
			
		_carController._Car = null;
		_carController.enabled = false;
	}

	public override void ConnectRacerCursorControllerTo(RacerCursor cursor)
	{
		
	}

	public override void DisconnectRacerCursorControllerFromCursor()
	{
		
	}
}
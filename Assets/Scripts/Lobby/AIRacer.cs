using Lobby;
using UnityEngine;

public class AIRacer : MonoBehaviour, IRacer
{
	private RacerScore _score = new();
	
	public RaceCar Car { get; set; }
	
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void ConnectCarControllerTo(RaceCar car)
	{
		
	}

	public void DisconnectCarControllerFromCar()
	{
		
	}

	public void ConnectRacerCursorControllerTo(RacerCursor cursor)
	{
		
	}

	public void DisconnectRacerCursorControllerFromCursor()
	{
		
	}

	public RacerScore GetScore()
		=> _score;
}
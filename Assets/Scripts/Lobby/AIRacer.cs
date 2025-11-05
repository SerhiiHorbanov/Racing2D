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

	public void EnableCarController(RaceCar car)
	{
		
	}

	public void DisableCarController()
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
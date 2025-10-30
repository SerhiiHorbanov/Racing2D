using Lobby;
using UnityEngine;

public class AIRacer : MonoBehaviour, IRacer
{
	private RacerScore _score;
	
	public RaceCar Car { get; set; }
	
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void SpawnController(RaceCar car)
	{
		
	}

	public RacerScore GetScore()
		=> _score;
}
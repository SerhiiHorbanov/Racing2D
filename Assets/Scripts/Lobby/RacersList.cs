using System;
using System.Collections.Generic;
using Lobby;
using UnityEngine;

public class RacersList : MonoBehaviour
{
	public List<Racer> Racers = new();
	public static RacersList Instance;
	
	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		
		DontDestroyOnLoad(gameObject);
		Instance = this;
	}

	public void Add(Racer racer)
		=> Racers.Add(racer);
	
	public bool Remove(Racer racer)
		=> Racers.Remove(racer);
}
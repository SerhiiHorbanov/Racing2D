using System;
using System.Collections.Generic;
using Lobby;
using UnityEngine;

public class RacersList : MonoBehaviour
{
	public List<IRacer> Racers = new();
	public static RacersList Instance;
	
	private void Awake()
	{
		if (Instance is not null)
		{
			Destroy(gameObject);
			return;
		}
		
		DontDestroyOnLoad(gameObject);
		Instance = this;
	}

	public void Add(IRacer racer)
		=> Racers.Add(racer);
	
	public void Remove(IRacer racer)
		=> Racers.Remove(racer);
}
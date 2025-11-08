using System;
using UnityEngine;

public class RacerScore
{
	public int Loops { get; private set; }
	private float _driftedDistance;
	
	public Action<int> OnScoreChanged;
	
	public void AddLoop()
	{
		Loops++;
		OnScoreChanged?.Invoke(Loops);
	}
	
	public void AddDrifting(float driftedDistance)
	{
		_driftedDistance += driftedDistance;
	}
}
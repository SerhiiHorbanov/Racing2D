using System;
using Data;
using UnityEngine;

public class RacerScore
{
	private int _roundWins;
	public int Loops;
	private float _driftedDistance;

	public ScoreMultipliers AppliedMultipliers;
	public Action<int> OnScoreChanged;// Parameter is the new score rounded
	
	public void AddLoop()
	{
		Loops++;
		InvokeOnScoreChanged();
	}

	private int CalculateRoundedScore()
		=> Mathf.RoundToInt(CalculateScore());

	private float CalculateScore()
	{
		float score = 0;
		
		score += _driftedDistance * AppliedMultipliers._DriftMultiplier;
		score += Loops * AppliedMultipliers._LoopsMultiplier;
		score += _roundWins * AppliedMultipliers._RoundWinsMultiplier;
		
		return score;
	}

	public void AddDrifting(float driftedDistance)
	{
		_driftedDistance += driftedDistance;
	}

	public void AddWin()
	{
		_roundWins++;
		InvokeOnScoreChanged();
	}
	
	private void InvokeOnScoreChanged()
	{
		OnScoreChanged?.Invoke(CalculateRoundedScore());	
	}
}
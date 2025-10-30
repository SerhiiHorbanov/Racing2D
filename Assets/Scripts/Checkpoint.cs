using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Checkpoint : MonoBehaviour
{
    [NonSerialized] public int Index;
    
    public Action<RaceCar, int> OnCarEntered;
    public Action<RaceCar, int> OnCarExited;

    private void OnTriggerEnter2D(Collider2D other)
    {
        RaceCar car = other.GetComponent<RaceCar>();
        
        if (car is null)
            return;
        
        OnCarEntered?.Invoke(car, Index);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        RaceCar car = other.GetComponent<RaceCar>();
        
        if (car is null)
            return;
        
        OnCarExited?.Invoke(car, Index);
    }
}

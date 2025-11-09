using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Score", menuName = "Scriptable Objects/Score")]
    public class ScoreMultipliers : ScriptableObject
    {
        [SerializeField] public float _LoopsMultiplier;
        [SerializeField] public float _RoundWinsMultiplier;
        [SerializeField] public float _DriftMultiplier;
    }
}

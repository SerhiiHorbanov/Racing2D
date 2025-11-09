using System;
using Lobby;
using UnityEngine;

namespace UI
{
    public class RacerPanel : MonoBehaviour
    {
        public IRacer Racer;

        public Action<IRacer> RemoveRacer;
    
        [SerializeField] private GameObject _AiText;
        
        public void SetRacer(IRacer racer)
        {
            Racer = racer;
            
            _AiText.SetActive(racer is AIRacer);
        }

        public void InvokeRemoveRacer()
        {
            RemoveRacer?.Invoke(Racer);
        }
    }
}

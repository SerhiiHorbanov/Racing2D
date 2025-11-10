using System;
using Lobby;
using UnityEngine;

namespace UI
{
    public class RacerPanel : MonoBehaviour
    {
        public Racer Racer;

        public Action<Racer> RemoveRacer;
    
        [SerializeField] private GameObject _AiText;
        
        public void SetRacer(Racer racer)
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

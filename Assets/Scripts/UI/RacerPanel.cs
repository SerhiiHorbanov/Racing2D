using System;
using Lobby;
using UnityEngine;

namespace UI
{
    public class RacerPanel : MonoBehaviour
    {
        public IRacer Racer;

        public Action<IRacer> RemoveRacer;
        
        public void SetRacer(IRacer racer)
        {
            Racer = racer;
        }

        public void InvokeRemoveRacer()
        {
            RemoveRacer?.Invoke(Racer);
        }
    }
}

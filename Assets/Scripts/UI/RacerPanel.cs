using System;
using Lobby;
using TMPro;
using UnityEngine;

namespace UI
{
    public class RacerPanel : MonoBehaviour
    {
        public Racer Racer;

        public Action<Racer> RemoveRacer;
    
        [SerializeField] private GameObject _AiText;
        [SerializeField] private TMP_InputField _ColorInputField;
        
        public void SetConfigurationColor(string inputFieldText)
        {
            string[] inputs = inputFieldText.Split(' ');
            
            if (inputs.Length < 3) 
                return;

            if (int.TryParse(inputs[0], out int r) &&
                int.TryParse(inputs[1], out int g) &&
                int.TryParse(inputs[2], out int b))
                Racer.Configuration.Color = new(r / 255.0f, g / 255.0f, b / 255.0f, 255);
        }
        
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

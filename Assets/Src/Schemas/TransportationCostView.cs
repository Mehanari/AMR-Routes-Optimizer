using System;
using Src.Model;
using TMPro;
using UnityEngine;

namespace Src.Schemas
{
    public class TransportationCostView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fromWorkstationName;
        [SerializeField] private TextMeshProUGUI toWorkstationName;
        [SerializeField] private TMP_InputField costInput;
        private TransportationCost _transportationCost;
        
        public TransportationCost TransportationCost => _transportationCost;

        private void Start()
        {
            costInput.onValueChanged.AddListener(OnCostChanged);
        }

        private void OnCostChanged(string arg0)
        {
            if (!int.TryParse(arg0, out var cost))
            {
                costInput.text = _transportationCost.Cost.ToString();
            }
            else
            {
                if(cost < 0)
                {
                    cost = 0;
                    costInput.text = cost.ToString();
                }
                _transportationCost.Cost = cost;
            }
        }

        public void Init(TransportationCost transportationCost)
        {
            _transportationCost = transportationCost;
            UpdateText();
        }

        public void UpdateText()
        {
            fromWorkstationName.text = _transportationCost.FromStation.Name;
            toWorkstationName.text = _transportationCost.ToStation.Name;
            costInput.text = _transportationCost.Cost.ToString();
        }
    }
}

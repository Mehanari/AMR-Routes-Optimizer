using Src.Model;
using TMPro;
using UnityEngine;

namespace Src.Schemas
{
    public class DepotTransportationCostView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI workstationName;
        [SerializeField] private TMP_InputField costInput;
        private WorkStation _workStation;

        private void Start()
        {
            costInput.onValueChanged.AddListener(OnCostChanged);
        }

        private void OnCostChanged(string newCost)
        {
            if (!int.TryParse(newCost, out var cost))
            {
                costInput.text = _workStation.DepotDistance.ToString();
            }
            else
            {
                if (cost < 0)
                {
                    costInput.text = _workStation.DepotDistance.ToString();
                }
                else
                {
                    _workStation.DepotDistance = cost;
                }
            }
        }

        public void Init(WorkStation workStation)
        {
            _workStation = workStation;
            UpdateText();
        }

        public void UpdateText()
        {
            workstationName.text = _workStation.Name;
            costInput.text = _workStation.DepotDistance.ToString();
        }
    }
}

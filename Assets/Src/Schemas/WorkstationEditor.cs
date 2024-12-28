using System;
using System.Linq;
using Src.Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Src.Schemas
{
    public class WorkstationEditor : MonoBehaviour
    {
        [SerializeField] private Button deleteButton;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_InputField demandInput;
        [SerializeField] private GameObject takenNameWarning;
        [SerializeField] private GameObject emptyNameWarning;
        private Schema _schema;
        private WorkStation _workStation;
        private WorkstationBehaviour _workstationBehaviour;

        public event Action<(WorkStation, WorkstationBehaviour)> DeleteClicked;

        private void Start()
        {
            nameInput.onValueChanged.AddListener(OnNameChanged);
            demandInput.onValueChanged.AddListener(OnDemandChanged);
            deleteButton.onClick.AddListener(OnDeleteButtonClick);
        }

        private void OnDeleteButtonClick()
        {
            DeleteClicked?.Invoke((_workStation, _workstationBehaviour));
            gameObject.SetActive(false);
        }

        public void SetSchema(Schema schema)
        {
            _schema = schema;
        }

        public void SetWorkstation(WorkStation workStation, WorkstationBehaviour behaviour)
        {
            _workStation = workStation;
            _workstationBehaviour = behaviour;
            nameInput.text = workStation.Name;
            demandInput.text = workStation.Demand.ToString();
        }

        private void OnDemandChanged(string demandStr)
        {
            if (!int.TryParse(demandStr, out var demand))
            {
                demandInput.text = _workStation.Demand.ToString();
            }
            else
            {
                _workStation.Demand = demand;
                _workstationBehaviour.UpdateText();
            }
        }

        private void OnNameChanged(string newName)
        {
            takenNameWarning.SetActive(false);
            if (string.IsNullOrEmpty(newName))
            {
                emptyNameWarning.SetActive(true);
                return;
            }
            var nameIsTaken = _schema.WorkStations.Any(w => w.Name == newName && !Equals(w, _workStation));
            if (nameIsTaken)
            {
                takenNameWarning.SetActive(true);
                return;
            }
            _workStation.Name = newName;
            _workstationBehaviour.UpdateText();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Src.Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Src.Schemas
{
    public class SchemaEditor : MonoBehaviour
    {
        [SerializeField] private Button addWorkstationButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private TMP_InputField amrQuantityInput;
        [SerializeField] private TMP_InputField amrCapacityInput;
        [SerializeField] private WorkstationEditor workstationEditor;
        [SerializeField] private WorkstationBehaviour workstationPrefab;
        [SerializeField] private GameObject serviceErrorMessage;
        [SerializeField] private TransportationCostView transportationCostPrefab;
        [SerializeField] private Transform transportationCostsContainer;
        [SerializeField] private DepotTransportationCostView depotTransportationCostPrefab;
        [SerializeField] private Transform depotTransportationCostsContainer;

        [SerializeField] private UnityEvent schemaUpdated = new UnityEvent();
        
        private SchemaSaver _schemaSaver;
        private WorkstationsBehaviourContainer _workstationsBehaviourContainer;
        private readonly List<TransportationCostView> _transportationCostsViews = new();
        private readonly List<DepotTransportationCostView> _depotTransportationCostViews = new();
        private Schema _schema;

        public void Init(WorkstationsBehaviourContainer workstationsBehaviourContainer, SchemaSaver schemaSaver)
        {
            _schemaSaver = schemaSaver;
            _workstationsBehaviourContainer = workstationsBehaviourContainer;
        }
        
        private void Start()
        {
            addWorkstationButton.onClick.AddListener(OnAddWorkstationButtonClick);
            saveButton.onClick.AddListener(OnSaveButtonClick);
            workstationEditor.DeleteClicked += OnWorkstationDelete;
            workstationEditor.WorkStationUpdated += schemaUpdated.Invoke;
            amrQuantityInput.onValueChanged.AddListener(OnAmrQuantityChanged);
            amrCapacityInput.onValueChanged.AddListener(OnAmrCapacityChanged);
        }

        private void OnAmrCapacityChanged(string arg0)
        {
            if (!int.TryParse(arg0, out var capacity))
            {
                amrCapacityInput.text = _schema.AmrParameters.Capacity.ToString();
            }
            else
            {
                if (capacity < 0)
                {
                    capacity = 0;
                    amrCapacityInput.text = capacity.ToString();
                }

                _schema.AmrParameters.Capacity = capacity;
                schemaUpdated.Invoke();
            }
        }

        private void OnAmrQuantityChanged(string arg0)
        {
            if (!int.TryParse(arg0, out var quantity))
            {
                amrQuantityInput.text = _schema.AmrParameters.Quantity.ToString();
            }
            else
            {
                if (quantity < 0)
                {
                    quantity = 0;
                    amrQuantityInput.text = quantity.ToString();
                }

                _schema.AmrParameters.Quantity = quantity;
                schemaUpdated.Invoke();
            }
        }

        private void OnWorkstationDelete((WorkStation workstation, WorkstationBehaviour behaviour) obj)
        {
            _schema.WorkStations.RemoveWhere(w => Equals(w, obj.workstation));
            _schema.TransportationCosts.RemoveWhere(t => Equals(t.FromStation, obj.workstation) || Equals(t.ToStation, obj.workstation));
            foreach (var costsView in _transportationCostsViews)
            {
                if (Equals(costsView.TransportationCost.FromStation, obj.workstation) || Equals(costsView.TransportationCost.ToStation, obj.workstation))
                {
                    Destroy(costsView.gameObject);
                }
            }
            _transportationCostsViews.RemoveAll(t => Equals(t.TransportationCost.FromStation, obj.workstation) || Equals(t.TransportationCost.ToStation, obj.workstation));
            _workstationsBehaviourContainer.DeleteBehaviorForWorkstation(obj.workstation);
            schemaUpdated.Invoke();
        }

        private async void OnSaveButtonClick()
        {
            if (_schema == null)
            {
                return;
            }
            await SaveSchema();
        }
        
        public async Task SaveSchema()
        {
            await _schemaSaver.SaveSchema(_schema);
        }

        private void OnAddWorkstationButtonClick()
        {
            var workStation = new WorkStation()
            {
                Name = GetFreeWorkstationName(),
                Demand = 0,
                X = 0,
                Y = 200
            };
            AddWorkstation(workStation);
        }

        private void AddWorkstation(WorkStation workStation)
        {            
            _schema.WorkStations.Add(workStation);
            AddWorkStationBehaviour(workStation);
            AddDepotTransportationCostView(workStation);
            var transportationCosts = new List<TransportationCost>();
            foreach (var otherWorkstation in _schema.WorkStations)
            {
                if (!Equals(otherWorkstation, workStation))
                {
                    var transportationCost = new TransportationCost()
                    {
                        FromStation = workStation,
                        ToStation = otherWorkstation,
                        Cost = 0
                    };
                    AddTransportationCosView(transportationCost);
                    transportationCosts.Add(transportationCost);
                }
            }
            _schema.TransportationCosts.AddRange(transportationCosts);
            schemaUpdated.Invoke();
        }
        
        private void AddDepotTransportationCostView(WorkStation workStation)
        {
            var depotTransportationCostView = Instantiate(depotTransportationCostPrefab, depotTransportationCostsContainer);
            depotTransportationCostView.Init(workStation);
            _depotTransportationCostViews.Add(depotTransportationCostView);
        }
        
        private void AddWorkStationBehaviour(WorkStation workStation)
        {
            var workstationBehaviour = Instantiate(workstationPrefab, transform);
            workstationBehaviour.Init(workStation, workstationEditor);
            workstationBehaviour.PositionChanged += schemaUpdated.Invoke;
            _workstationsBehaviourContainer.AddWorkstationBehaviour(workstationBehaviour);
        }
        
        private void AddTransportationCosView(TransportationCost transportationCost)
        {
            var transportationCostView = Instantiate(transportationCostPrefab, transportationCostsContainer);
            transportationCostView.Init(transportationCost);
            _transportationCostsViews.Add(transportationCostView);
        }

        private string GetFreeWorkstationName()
        {
            var name = "WK" + _schema.WorkStations.Count;
            var i = 0;
            while (_schema.WorkStations.Any(ws => ws.Name == name))
            {
                i++;
                name = "WK" + _schema.WorkStations.Count + i;
            }

            return name;
        }

        public void LoadSchema(Schema schema)
        {
            _schema = schema;
            workstationEditor.SetSchema(schema);
            foreach (var workStation in _schema.WorkStations)
            {
                AddWorkStationBehaviour(workStation);
                AddDepotTransportationCostView(workStation);
            }
            foreach (var transportationCost in _schema.TransportationCosts)
            {
                AddTransportationCosView(transportationCost);
            }
            amrCapacityInput.text = _schema.AmrParameters.Capacity.ToString();
            amrQuantityInput.text = _schema.AmrParameters.Quantity.ToString();
        }

        public void Clear()
        {
            _workstationsBehaviourContainer.Clear();
            foreach (var costView in _transportationCostsViews)
            {
                Destroy(costView.gameObject);
            }
            _transportationCostsViews.Clear();
            foreach (var depotTransportationCostView in _depotTransportationCostViews)
            {
                Destroy(depotTransportationCostView.gameObject);
            }
            _depotTransportationCostViews.Clear();
            _schema = null;
            serviceErrorMessage.SetActive(false);
        }

        public void CalculateTransportationCosts()
        {
            foreach (var cost in _schema.TransportationCosts)
            {
                var from = cost.FromStation;
                var fromStationPosition = new Vector2(from.X, from.Y);
                var to = cost.ToStation;
                var toStationPosition = new Vector2(to.X, to.Y);
                var distance = Vector2.Distance(fromStationPosition, toStationPosition);
                cost.Cost = (int)distance;
            }
            foreach (var costView in _transportationCostsViews)
            {
                costView.UpdateText();
            }
            foreach (var workStation in _schema.WorkStations)
            {
                var depotPosition = Vector2.zero;
                var depotDistance = Vector2.Distance(new Vector2(workStation.X, workStation.Y), depotPosition);
                workStation.DepotDistance = (int)depotDistance;
            }
            foreach (var depotCostView in _depotTransportationCostViews)
            {
                depotCostView.UpdateText();
            }
            schemaUpdated.Invoke();
        }
    }
}
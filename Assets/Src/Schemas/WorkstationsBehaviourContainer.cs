using System.Collections.Generic;
using JetBrains.Annotations;
using Src.Model;
using UnityEngine;

namespace Src.Schemas
{
    public class WorkstationsBehaviourContainer : MonoBehaviour
    {
        private readonly List<WorkstationBehaviour> _workstationsBehaviours = new();
        
        public void AddWorkstationBehaviour(WorkstationBehaviour workstationBehaviour)
        {
            _workstationsBehaviours.Add(workstationBehaviour);
        }

        public void DeleteBehaviorForWorkstation(WorkStation workStation)
        {
            var workstationBehaviour = _workstationsBehaviours.Find(wb => Equals(wb.WorkStation, workStation));
            if (workstationBehaviour != null)
            {
                _workstationsBehaviours.Remove(workstationBehaviour);
                Destroy(workstationBehaviour.gameObject);
            }
        }
        
        [CanBeNull]
        public WorkstationBehaviour GetBehaviourForWorkstation(WorkStation workStation)
        {
            return _workstationsBehaviours.Find(wb => Equals(wb.WorkStation, workStation));
        }
        
        public WorkstationBehaviour GetBehaviourByWorkstationName(string workstationName)
        {
            return _workstationsBehaviours.Find(wb => wb.WorkStation.Name == workstationName);
        }

        public void Clear()
        {
            foreach (var workstationBehaviour in _workstationsBehaviours)
            {
                Destroy(workstationBehaviour.gameObject);
            }

            _workstationsBehaviours.Clear();
        }
    }
}
using System;
using Src.Model;
using TMPro;
using UnityEngine;

namespace Src.Schemas
{
    public class WorkstationBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshPro nameText;
        [SerializeField] private TextMeshPro demandText;
        
        private WorkStation _workStation;
        private WorkstationEditor _editor;
        private Camera _camera;
        private const int Decimals = 100;
        private Vector3 _offset;
        
        private void Start()
        {
            _camera = Camera.main;
        }
        
        public void Init(WorkStation workStation, WorkstationEditor editor)
        {
            _editor = editor;
            _workStation = workStation;
            nameText.text = workStation.Name;
            demandText.text = "Demand: " + workStation.Demand;
            transform.position = new Vector3(workStation.X/(float)Decimals, workStation.Y/(float)Decimals, 0);
        }

        public void UpdateText()
        {
            nameText.text = _workStation.Name;
            demandText.text = "Demand: " + _workStation.Demand;
        }
        
        private void OnMouseDrag()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                Vector3 mousePositionWorld = _camera.ScreenToWorldPoint(mousePosition);
                Vector3 roundedPosition = Round(mousePositionWorld, Decimals);
                roundedPosition += _offset;
                _workStation.X = (int)(roundedPosition.x * Decimals);
                _workStation.Y = (int)(roundedPosition.y * Decimals);
                transform.position = roundedPosition;
            }
        }

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                Vector3 mousePositionWorld = _camera.ScreenToWorldPoint(mousePosition);
                Vector3 roundedPosition = Round(mousePositionWorld, Decimals);
                _offset = transform.position - roundedPosition;
            }
        }

        private void OnMouseOver()
        {
            if(Input.GetMouseButtonDown(1))
            {
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                Vector3 mousePositionWorld = _camera.ScreenToWorldPoint(mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePositionWorld, Vector2.zero);
                Debug.Log("Right click on workstation: " + _workStation.Name);
                _editor.gameObject.SetActive(true);
                _editor.SetWorkstation(_workStation, this);
            }
        }

        private void OnMouseUp()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _offset = Vector3.zero;
            }
        }

        private Vector3 Round(Vector3 vector, int decimals)
        {
            return new Vector3(Mathf.Round(vector.x * decimals) / decimals, Mathf.Round(vector.y * decimals) / decimals, Mathf.Round(vector.z * decimals) / decimals);
        }
    }
}
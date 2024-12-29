using UnityEngine;
using UnityEngine.InputSystem;

namespace Src
{
    public class ScrollZoomController : MonoBehaviour
    {
        [SerializeField] private float _scrollZoomScaling = 1.1f;
        [SerializeField] private float _minCameraSize = 1;
        [SerializeField] private float _maxCameraSize = 10;
        [SerializeField] private float _defaultCameraSize = 5;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }
        
        public void ResetCamera()
        {
            _camera.orthographicSize = _defaultCameraSize;
        }

        void Update()
        {
            var scrollDelta = Mouse.current.scroll.ReadValue();
            if (scrollDelta.y != 0)
            {
                var cameraTransform = _camera.transform;
                var cameraPosition = cameraTransform.position;
                var mousePosition = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                var oldSize = _camera.orthographicSize;
                if (scrollDelta.y < 0)
                {
                    _camera.orthographicSize /= _scrollZoomScaling;
                    if (_camera.orthographicSize > _maxCameraSize)
                    {
                        _camera.orthographicSize = _maxCameraSize;
                    }
                    
                }
                else if(scrollDelta.y > 0)
                {
                    _camera.orthographicSize *= _scrollZoomScaling;
                    if (_camera.orthographicSize < _minCameraSize)
                    {
                        _camera.orthographicSize = _minCameraSize;
                    }
                }

                var newSize = _camera.orthographicSize;
                var ratio = newSize / oldSize;
                var cameraNewPosition = mousePosition - (mousePosition - cameraPosition)*ratio;
                cameraTransform.position = cameraNewPosition;
            }
        }
    }
}

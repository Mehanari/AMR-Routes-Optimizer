using UnityEngine;

namespace Src
{
    public class WASDMovement : MonoBehaviour
    {
        [SerializeField] private float verticalSpeed = 1;
        [SerializeField] private float horizontalSpeed = 1;
        [SerializeField] private Vector3 startPosition;
        
        private void Update()
        {
            var vertical = Input.GetAxis("Vertical") * verticalSpeed * Time.deltaTime;
            var horizontal = Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime;
            transform.Translate(horizontal, vertical, 0);
        }
        
        public void ResetPosition()
        {
            transform.position = startPosition;
        }
    }
}

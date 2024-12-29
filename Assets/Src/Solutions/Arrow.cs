using UnityEngine;

namespace Src.Solutions
{
    public class Arrow : MonoBehaviour
    { 
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private GameObject head;
        [SerializeField] private SpriteRenderer headSpriteRenderer;
        [SerializeField] private float width;
        
        private void Start()
        {
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
        }
        
        public void SetPositions(Vector3 start, Vector3 end, float headOffset)
        {
            lineRenderer.SetPosition(0, start);
            var directionV3 = end - start;
            var direction = new Vector2(directionV3.x, directionV3.y);
            var offset = direction.normalized * headOffset;
            head.transform.position = end - new Vector3(offset.x, offset.y, 0);
            var left = new Vector2(1, 0);
            var product = Vector2.Dot(direction, left);
            var angle = Mathf.Acos(product / (direction.magnitude * left.magnitude)) * Mathf.Rad2Deg;
            if (end.y > start.y)
            {
                angle -= 90;
            }
            else
            {
                angle = 360 - angle - 90;
            }
            head.transform.rotation = Quaternion.Euler(0, 0, angle);
            lineRenderer.SetPosition(1, headSpriteRenderer.transform.position);
        }
        
        public void SetColor(Color color)
        {
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            headSpriteRenderer.color = color;
        }
    }
}
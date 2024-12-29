using UnityEngine;

namespace Src.Solutions
{
    public class ArrowTest : MonoBehaviour
    {
        [SerializeField] private Arrow arrow;
        [SerializeField] private float headOffset;
        [SerializeField] private Color color;
        [SerializeField] private Transform start;
        [SerializeField] private Transform end;
        
        private void Start()
        {
            arrow.SetPositions(start.position, end.position, headOffset);
            arrow.SetColor(color);
        }
    }
}
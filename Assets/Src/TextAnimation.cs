using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Src
{
    public class TextAnimation : MonoBehaviour
    {
        [SerializeField] private List<string> texts;
        [SerializeField] private float delay = 1f;
        [SerializeField] private TextMeshProUGUI text;
        
        private void Update()
        {
            var textIndex = (int) (Time.time / delay) % texts.Count;
            text.text = texts[textIndex];
        }
    }
}

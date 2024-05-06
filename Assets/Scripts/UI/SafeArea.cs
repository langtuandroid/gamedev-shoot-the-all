using UnityEngine;

namespace UI
{
    public class SafeArea : MonoBehaviour
    {
        [SerializeField] private bool _dontSafeBottom;
        [SerializeField] private  float _downOffset = 0.1f;
       
        private RectTransform fittedRectTransform;
        private Rect safeRectComponent;
        private Vector2 minAnchorVector;
        private Vector2 maxAnchorVector;

        private void Awake()
        {
            fittedRectTransform = GetComponent<RectTransform>();
            safeRectComponent = Screen.safeArea;
            minAnchorVector = safeRectComponent.position;
            maxAnchorVector = minAnchorVector + safeRectComponent.size;
        
            minAnchorVector.x /= Screen.width;
            minAnchorVector.y = _dontSafeBottom ? minAnchorVector.y = 0 : _downOffset;
            maxAnchorVector.x /= Screen.width;
            maxAnchorVector.y /= Screen.height;
        
            fittedRectTransform.anchorMin = minAnchorVector;
            fittedRectTransform.anchorMax = maxAnchorVector;
        }
    }
}
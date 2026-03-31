using UnityEngine;
using UnityEngine.EventSystems;

namespace SplineCurves
{
    public class DraggableNode : MonoBehaviour, IDragHandler
    {
        public float sliderMin;             
        public float sliderMax;          
        public RectTransform sliderRect;    
        public System.Action<float> onNodeMoved; 

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(sliderRect, eventData.position, null, out localPoint);

            float t = Mathf.Clamp(
                (localPoint.x / sliderRect.sizeDelta.x) * (sliderMax - sliderMin) + sliderMin,
                sliderMin,
                sliderMax
            );

            transform.localPosition = new Vector3(localPoint.x, transform.localPosition.y, 0);
            onNodeMoved?.Invoke(t);
        }
    }
}
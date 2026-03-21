using UnityEngine;
namespace BezierCurves 
{
    public class DraggablePoint : MonoBehaviour
    {
        private CurveSystem curveSystem;
        private Vector3 offset;
        private float distance;

        void OnMouseDown()
        {
            Debug.Log("Clicked!");
            distance = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = distance;
            offset = transform.position - Camera.main.ScreenToWorldPoint(mousePos);
        }

        public void SetCurveSystem(CurveSystem system)
        {
            curveSystem = system;
        }

        void OnMouseDrag()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = distance;
            transform.position = Camera.main.ScreenToWorldPoint(mousePos) + offset;

            curveSystem.SetCurveDirty();
        }
    }
}
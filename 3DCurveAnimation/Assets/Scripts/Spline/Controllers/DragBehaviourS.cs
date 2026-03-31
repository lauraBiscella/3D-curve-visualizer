using UnityEngine;

namespace SplineCurves
{
    public class DraggablePoint : MonoBehaviour
    {
        private CurveSystem curveSystem;
        private Plane dragPlane;
        private Camera cam;

        public void SetCurveSystem(CurveSystem system)
        {
            curveSystem = system;
        }

        void Awake()
        {
            cam = Camera.main;
        }

        void OnMouseDown()
        {
            dragPlane = new Plane(-cam.transform.forward, transform.position);
        }

        void OnMouseDrag()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            float distance;

            if (dragPlane.Raycast(ray, out distance))
            {
                transform.position = ray.GetPoint(distance);

                if (curveSystem != null)
                    curveSystem.SetCurveDirty();
            }
        }
    }
}
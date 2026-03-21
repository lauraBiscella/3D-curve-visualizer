using UnityEngine;
using System.Collections.Generic;

namespace BezierCurves
{
    public class CurveRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject controlPolygonPrefab;
        [SerializeField] private GameObject bezierCurvePrefab; 
        private LineRenderer controlPolygon;
        private LineRenderer bezierCurve;
        private bool controlPolygonInstanciated = false;
        private bool bezierCurveInstanciated = false;

        public void DrawControlPolygon(List<Transform> points)
        {
            if(!controlPolygonInstanciated)
            {
                GameObject cp = Instantiate(controlPolygonPrefab, Vector3.zero, Quaternion.identity);
                controlPolygon = cp.GetComponent<LineRenderer>();
                
                controlPolygonInstanciated = true;
            }

            controlPolygon.positionCount = points.Count;

            for (int i = 0; i < points.Count; i++)
                controlPolygon.SetPosition(i, points[i].position);
        }

        public void DrawBezierCurve(List<Transform> points, int resolution)
        {
            if(!bezierCurveInstanciated)
            {
                GameObject bc = Instantiate(bezierCurvePrefab, Vector3.zero, Quaternion.identity);
                bezierCurve = bc.GetComponent<LineRenderer>();
                
                bezierCurveInstanciated = true;
            }

            bezierCurve.positionCount = resolution;

            for (int i = 0; i < resolution; i++)
            {
                float t = i / (float)(resolution - 1);
                Vector3 p = BezierMath.BezierPoint(t, points);
                bezierCurve.SetPosition(i, p);
            }
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

namespace SplineCurves
{
    public class CurveRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject deBoorPolygonPrefab;
        [SerializeField] private GameObject bSplineCurvePrefab; 
        [SerializeField] private GameObject controlPolygonPrefab;
        [SerializeField] private GameObject controlPointTickPrefab;
        private LineRenderer deBoorPolygon;
        private LineRenderer bSplineCurve;
        private LineRenderer controlPolygon;
        private bool deBoorPolygonInstanciated = false;
        private bool bSplineCurveInstanciated = false;
        private bool controlPolygonInstanciated = false;
        private List<LineRenderer> controlPointTicks = new List<LineRenderer>();

        public void DrawDeBoorPolygon(List<Transform> points)
        {
            if(!deBoorPolygonInstanciated)
            {
                GameObject dbp = Instantiate(deBoorPolygonPrefab, Vector3.zero, Quaternion.identity);
                deBoorPolygon = dbp.GetComponent<LineRenderer>();
                
                deBoorPolygonInstanciated = true;
            }

            deBoorPolygon.positionCount = points.Count;

            for (int i = 0; i < points.Count; i++)
                deBoorPolygon.SetPosition(i, points[i].position);
        }

        public void DrawControlPolygon(List<Transform> points, float[] knots)
        {
            if(!controlPolygonInstanciated)
            {
                GameObject cp = Instantiate(controlPolygonPrefab, Vector3.zero, Quaternion.identity);
                controlPolygon = cp.GetComponent<LineRenderer>();
                controlPolygonInstanciated = true;
            }

            List<Vector3> deBoor = new List<Vector3>();

            foreach(var p in points)
                deBoor.Add(p.position);

            List<Vector3> controlPoints =
                BSplineMath.BSplineToBezier(deBoor, knots, 3);

            controlPolygon.positionCount = controlPoints.Count;

            for(int i = 0; i < controlPoints.Count; i++)
                controlPolygon.SetPosition(i, controlPoints[i]);
            
            DrawControlPointTicks(controlPoints);
        }

        public void DrawBSpline(List<Transform> deBoorPoints, float[] knots, int resolution, int degree)
        {
            if(!bSplineCurveInstanciated)
            {
                GameObject bc = Instantiate(bSplineCurvePrefab);
                bSplineCurve = bc.GetComponent<LineRenderer>();
                bSplineCurveInstanciated = true;
            }

            bSplineCurve.positionCount = resolution;
            float tStart = knots[degree];
            float tEnd = knots[deBoorPoints.Count];
            for (int i = 0; i < resolution; i++)
            {    
                float t = tStart + (tEnd - tStart) * (i / (float)(resolution - 1));
                Vector3 p = BSplineMath.BSplinePoint(t, deBoorPoints, knots, degree);
                bSplineCurve.SetPosition(i, p);
            }
        }

        void DrawControlPointTicks(List<Vector3> points)
        {
            float tickSize = 0.08f;

            // crea tick se non esistono
            while(controlPointTicks.Count < points.Count)
            {
                GameObject tick = Instantiate(controlPointTickPrefab);
                controlPointTicks.Add(tick.GetComponent<LineRenderer>());
            }

            for(int i=0;i<points.Count;i++)
            {
                Vector3 p = points[i];

                Vector3 a = p + Vector3.left * tickSize;
                Vector3 b = p + Vector3.right * tickSize;

                controlPointTicks[i].positionCount = 2;
                controlPointTicks[i].SetPosition(0, a);
                controlPointTicks[i].SetPosition(1, b);
            }
        }
    }
}
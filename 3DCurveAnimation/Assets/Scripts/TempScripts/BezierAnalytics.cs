using UnityEngine;
using System.Collections.Generic;
namespace BezierCurves
{
    public static class BezierAnalytics
    {
        public static float Curvature(Vector3 d1, Vector3 d2)
        {
            float denom = Mathf.Pow(d1.magnitude, 3);

            if (denom < 0.0001f)
                return 0;

            return Vector3.Cross(d1, d2).magnitude / denom;
        }

        public static float Torsion(Vector3 d1, Vector3 d2, Vector3 d3)
        {
            float crossMag = Vector3.Cross(d1, d2).magnitude;

            if (crossMag < 0.0001f)
                return 0;

            return Vector3.Dot(Vector3.Cross(d1, d2), d3) / (crossMag * crossMag);
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
namespace SplineCurves
{
    public static class BSplineAnalytics
    {
        public static float Curvature(Vector3 r1, Vector3 r2)
        {
            return Vector3.Cross(r1, r2).magnitude / Mathf.Pow(r1.magnitude, 3);
        }

        public static float Torsion(Vector3 r1, Vector3 r2, Vector3 r3)
        {
            Vector3 cross = Vector3.Cross(r1, r2);
            float denom = cross.sqrMagnitude;
            if (denom < 1e-6f) return 0f; // evita instabilità
            return Vector3.Dot(cross, r3) / denom;
        }

    }
}
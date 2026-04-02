using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace SplineCurves
{
    public class CurveInfoPanel : MonoBehaviour
    {
        [SerializeField] private CurveSystem curveSystem;
        [SerializeField] private TextMeshProUGUI pointText;
        [SerializeField] private TextMeshProUGUI curvatureText;
        [SerializeField] private TextMeshProUGUI torsionText;
        [SerializeField] private int degree;

        public void UpdateInfo(float t)
        {
            if (!curveSystem.HasEnoughElements()) return;

            List<Vector3> points = new List<Vector3>();
            foreach (Transform tr in curveSystem.deBoorPoints)
                points.Add(tr.position);
            var knots = curveSystem.knots;

            Vector3 p = BSplineMath.BSplinePoint(t, points, knots, degree);
            Vector3 d1 = BSplineMath.BSplineFirstDerivative(t, points, knots, degree);
            Vector3 d2 = BSplineMath.BSplineSecondDerivative(t, points, knots, degree);
            Vector3 d3 = BSplineMath.BSplineThirdDerivative(t, points, knots, degree);

            float curvature = BSplineAnalytics.Curvature(d1, d2);
            float torsion = BSplineAnalytics.Torsion(d1, d2, d3);
            
            if (pointText != null)
                pointText.text = $"({p.x:F2}, {p.y:F2}, {p.z:F2})";
            
            if (curvatureText != null)
                curvatureText.text = curvature.ToString("F2");

            if (torsionText != null)
                torsionText.text = torsion.ToString("F2");
            
        }
    }
}
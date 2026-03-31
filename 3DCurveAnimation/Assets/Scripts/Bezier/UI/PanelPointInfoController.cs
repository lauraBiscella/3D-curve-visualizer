using UnityEngine;
using TMPro;

namespace BezierCurves
{
    public class CurveInfoPanel : MonoBehaviour
    {
        [SerializeField] private CurveSystem curveSystem;
        [SerializeField] private TextMeshProUGUI pointText;
        [SerializeField] private TextMeshProUGUI curvatureText;
        [SerializeField] private TextMeshProUGUI torsionText;

        public void UpdateInfo(float t)
        {
            if (!curveSystem.HasEnoughElements()) return;

            var points = curveSystem.controlPoints;

            Vector3 p = BezierMath.BezierPoint(t, points);

            Vector3 d1 = BezierMath.FirstDerivative(t, points);
            Vector3 d2 = BezierMath.SecondDerivative(t, points);
            Vector3 d3 = BezierMath.ThirdDerivative(t, points);

            float curvature = BezierAnalytics.Curvature(d1, d2);
            float torsion = BezierAnalytics.Torsion(d1, d2, d3);

            if (pointText != null)
                pointText.text = $"({p.x:F2}, {p.y:F2}, {p.z:F2})";

            if (curvatureText != null)
                curvatureText.text = curvature.ToString("F2");

            if (torsionText != null)
                torsionText.text = torsion.ToString("F2");
        }
    }
}
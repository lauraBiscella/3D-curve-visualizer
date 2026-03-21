using UnityEngine;
using TMPro;

public class CurveInfoPanel : MonoBehaviour
{
    [SerializeField] private CurveSystem curveSystem;
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private TextMeshProUGUI curvatureText;
    [SerializeField] private TextMeshProUGUI torsionText;

    public void UpdateInfo(float t)
    {
        if (!curveSystem.HasFourElements()) return;

        Vector3 p = curveSystem.BezierPoint(t, curveSystem.controlPoints.Count - 1);

        float curvature = curveSystem.ComputeCurvatureSingleValue(t);
        float torsion = curveSystem.ComputeTorsionSingleValue(t);

        if (pointText != null)
            pointText.text = $"({p.x:F2}, {p.y:F2}, {p.z:F2})";

        if (curvatureText != null)
            curvatureText.text = $"{curvature:F3}";

        if (torsionText != null)
            torsionText.text = $"{torsion:F3}";
    }
}
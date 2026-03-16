using UnityEngine;
using System.Collections.Generic;

public class BezierAnalysis : CurveSystem
{
    public List<float> curvatureValues = new List<float>();
    public List<float> torsionValues = new List<float>();
    public GraphRenderer curvatureGraph;
    public GraphRenderer torsionGraph;  
    private List<Transform> controlPoints;
    protected override void Update()
    {
        if (HasFourElements()) // calcola solo quando ci sono 4 punti
        {
            controlPoints = new List<Transform>(GetPoints());
            ComputeValues();    
        }
        base.Update();
    }

    void ComputeValues()
    {
        curvatureValues.Clear();
        torsionValues.Clear();

        for(int i=0;i<curveResolution;i++)
        {
            float t = i/(float)(curveResolution-1);
            Vector3 d1 = FirstDerivative(t);
            Vector3 d2 = SecondDerivative(t);
            Vector3 d3 = ThirdDerivative();
            float denom1 = Mathf.Pow(d1.magnitude,3);
            float curvature = 0f;
            if(denom1 > 0.0001f)
            {
                curvature = Vector3.Cross(d1,d2).magnitude / denom1;
            }
            float crossMag = Vector3.Cross(d1,d2).magnitude;
            float torsion = 0f;

            if(crossMag > 0.0001f)
            {
                torsion = Vector3.Dot(Vector3.Cross(d1,d2),d3) / (crossMag*crossMag);
            }
            curvatureValues.Add(curvature);
            torsionValues.Add(torsion);
        }
        curvatureGraph.DrawGraph(curvatureValues);
        torsionGraph.DrawGraph(torsionValues);
    }

    Vector3 FirstDerivative(float t)
    {
        float u = 1-t;
        return
            3*u*u*(controlPoints[1].position-controlPoints[0].position) +
            6*u*t*(controlPoints[2].position-controlPoints[1].position) +
            3*t*t*(controlPoints[3].position-controlPoints[2].position);
    }

    Vector3 SecondDerivative(float t)
    {
        return
            6*(1-t)*(controlPoints[2].position-2*controlPoints[1].position+controlPoints[0].position) +
            6*t*(controlPoints[3].position-2*controlPoints[2].position+controlPoints[1].position);
    }

    Vector3 ThirdDerivative()
    {
        return 6*(controlPoints[3].position-3*controlPoints[2].position+3*controlPoints[1].position-controlPoints[0].position);
    }
}
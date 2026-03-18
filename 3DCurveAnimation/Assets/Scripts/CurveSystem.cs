using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CurveSystem : MonoBehaviour
{
    [SerializeField] private GameObject controlPointPrefab;
    [SerializeField] private GameObject controlPolygonPrefab;
    [SerializeField] private GameObject bezierCurvePrefab; 
    [SerializeField] private GraphRenderer curvatureGraph;
    [SerializeField] private GraphRenderer torsionGraph;  
    [SerializeField] private int curveResolution = 100;
    [SerializeField] private Slider tSlider;
    [SerializeField] private GameObject pointIndicatorPrefab;
    private List<Transform> controlPoints = new List<Transform>();
    private LineRenderer controlPolygon;
    private LineRenderer bezierCurve;
    private List<float> curvatureValues = new List<float>();
    private List<float> torsionValues = new List<float>();
    private bool lineRenderersInstantiated = false;
    private GameObject pointIndicator;

    void Start()
    {
        pointIndicator = Instantiate(pointIndicatorPrefab, new Vector3(-300, -300, 0), Quaternion.identity);
        tSlider.onValueChanged.AddListener(OnSliderChanged);
    }
    void Update()
    {
        HandleMouseClick();

        if(HasFourElements())
        {
            if(!lineRenderersInstantiated)
            {
                GameObject cp = Instantiate(controlPolygonPrefab, Vector3.zero, Quaternion.identity);
                controlPolygon = cp.GetComponent<LineRenderer>();

                GameObject bc = Instantiate(bezierCurvePrefab, Vector3.zero, Quaternion.identity);
                bezierCurve = bc.GetComponent<LineRenderer>();

                lineRenderersInstantiated = true;
            }
            DrawControlPolygon();
            DrawBezierCurve();
            DrawAnalytics();
        }
    }

    public bool HasFourElements()
    {
        return controlPoints.Count == 4;
    }   

    void HandleMouseClick()
    {
        if(Input.GetMouseButtonDown(0) && controlPoints.Count < 4)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                GameObject p = Instantiate(controlPointPrefab, hit.point, Quaternion.identity);
                controlPoints.Add(p.transform);
            }
        }
    }

    void OnSliderChanged(float t)
    {
        if (!HasFourElements()) return;
        Vector3 bezierPos = BezierPoint(t, controlPoints.Count - 1);
        pointIndicator.transform.position = bezierPos;

        if (curvatureGraph != null)
            curvatureGraph.SetMarkerNormalized(t); 
        if (torsionGraph != null)
            torsionGraph.SetMarkerNormalized(t); 
    }

    void DrawControlPolygon()
    {
        if (controlPolygon == null) return;
        controlPolygon.positionCount = controlPoints.Count;

        for(int i=0;i<controlPoints.Count;i++)
        {
            controlPolygon.SetPosition(i, controlPoints[i].position);
        }
    }

    void DrawBezierCurve()
    {
        if (bezierCurve == null) return;
        bezierCurve.positionCount = curveResolution;

        for(int i=0; i<curveResolution; i++)
        {
            float t = i/(float)(curveResolution-1);
            bezierCurve.SetPosition(i, BezierPoint(t, controlPoints.Count - 1));
        }
    }

    void DrawAnalytics()
    {
        ComputeAnalytics();
        curvatureGraph.DrawGraph(curvatureValues);
        torsionGraph.DrawGraph(torsionValues);
    }

    void ComputeAnalytics()
    {
        curvatureValues.Clear();
        torsionValues.Clear();
        float curvature = 0;
        float torsion = 0;

        for(int i=0;i<curveResolution;i++)
        {
            float t = i/(float)(curveResolution-1);
            Vector3 d1 = FirstDerivative(t);
            Vector3 d2 = SecondDerivative(t);
            Vector3 d3 = ThirdDerivative(t);
            curvature = ComputeCurvature(d1, d2);
            torsion = ComputeTorsion(d1, d2, d3);
            curvatureValues.Add(curvature);
            torsionValues.Add(torsion);
        }
    }

    float ComputeCurvature(Vector3 d1, Vector3 d2)
    {
        float denom1 = Mathf.Pow(d1.magnitude,3);
        float curvature = 0f;
        if(denom1 > 0.0001f)
        {
            curvature = Vector3.Cross(d1,d2).magnitude / denom1;
        }
        return curvature;
    }

    float ComputeTorsion(Vector3 d1, Vector3 d2, Vector3 d3)
    {
        float crossMag = Vector3.Cross(d1,d2).magnitude;
        float torsion = 0f;
        if(crossMag > 0.0001f)
        {
            torsion = Vector3.Dot(Vector3.Cross(d1,d2),d3) / (crossMag*crossMag);
        }
        return torsion;
    }

    Vector3 BezierPoint(float t, int k)
    {
        Vector3 point = new Vector3(0,0,0);
        for (int i=0; i <= k; i++)
        {
           float b = BernsteinPolynomial(k, i, t);
           point += controlPoints[i].position * b;
        }

        return point;
    }

    float Binomial(int n, int k)
    {
        float result = 1;
        for (int i = 1; i <= k; i++)
        {
            result *= n - (k-i);
            result /= i;
        }
        return result;
    }

    float BernsteinPolynomial(int k, int i, float t)
    {
        return Binomial(k, i) * Mathf.Pow(t, i) * Mathf.Pow(1-t, k-i);
    }

    Vector3 FirstDerivative(float t)
    {
        int n = controlPoints.Count - 1;
        Vector3 sum = Vector3.zero;
        for (int i = 0; i < n; i++)
        {
            sum += (controlPoints[i + 1].position - controlPoints[i].position) * BernsteinPolynomial(n - 1, i, t);
        }
        sum = sum * (float)n;
        return sum;
    }

    Vector3 SecondDerivative(float t)
    {
        int n = controlPoints.Count - 1;
        Vector3 sum = Vector3.zero;
        for (int i = 0; i < n - 1; i++)
        {
            sum += (controlPoints[i + 2].position - 2 * controlPoints[i + 1].position + controlPoints[i].position) * BernsteinPolynomial(n - 2, i, t);
        }
        sum = sum * (float)(n * (n - 1));
        return sum;
    }

    Vector3 ThirdDerivative(float t)
    {
        int n = controlPoints.Count - 1;
        Vector3 sum = Vector3.zero;
        for (int i = 0; i < n - 2; i++)
        {
            sum += (controlPoints[i + 3].position - 3 * controlPoints[i + 2].position + 3 * controlPoints[i + 1].position - controlPoints[i].position) * BernsteinPolynomial(n - 3, i, t);
        }
        sum = sum * (float)(n * (n - 1) * (n - 2));
        return sum;
    }
}
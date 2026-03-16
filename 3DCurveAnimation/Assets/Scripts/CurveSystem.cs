using UnityEngine;
using System.Collections.Generic;

public class CurveSystem : MonoBehaviour
{
    public GameObject controlPointPrefab;
    public GameObject controlPolygonPrefab;
    public GameObject bezierCurvePrefab;
    public LineRenderer controlPolygon;
    public LineRenderer bezierCurve;

    private List<Transform> points = new List<Transform>();

    public int curveResolution = 100;
    private bool lineRenderersInstantiated = false;

    protected virtual void Update()
    {
        HandleMouseClick();

        if(points.Count == 4)
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
        }
    }

    public bool HasFourElements()
    {
        return points.Count == 4;
    }   

    public List<Transform> GetPoints()
    {
        return points;
    }

    void HandleMouseClick()
    {
        if(Input.GetMouseButtonDown(0) && points.Count < 4)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                GameObject p = Instantiate(controlPointPrefab, hit.point, Quaternion.identity);
                points.Add(p.transform);
            }
        }
    }

    void DrawControlPolygon()
    {
        if (controlPolygon == null) return;
        controlPolygon.positionCount = points.Count;

        for(int i=0;i<points.Count;i++)
        {
            controlPolygon.SetPosition(i, points[i].position);
        }
    }

    void DrawBezierCurve()
    {
        if (bezierCurve == null) return;
        bezierCurve.positionCount = curveResolution;

        for(int i=0;i<curveResolution;i++)
        {
            float t = i/(float)(curveResolution-1);
            Vector3 pos = BezierPoint(t);
            bezierCurve.SetPosition(i,pos);
        }
    }

    Vector3 BezierPoint(float t)
    {
        float u = 1-t;

        return
            u*u*u*points[0].position +
            3*u*u*t*points[1].position +
            3*u*t*t*points[2].position +
            t*t*t*points[3].position;
    }
}
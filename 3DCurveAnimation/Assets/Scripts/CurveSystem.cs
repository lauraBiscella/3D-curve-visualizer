using UnityEngine;
using System.Collections.Generic;

public class CurveSystem : MonoBehaviour
{
    public GameObject controlPointPrefab;

    public LineRenderer controlPolygon;
    public LineRenderer bezierCurve;

    private List<Transform> points = new List<Transform>();

    public int curveResolution = 100;

    void Update()
    {
        HandleMouseClick();

        if(points.Count == 4)
        {
            DrawControlPolygon();
            DrawBezierCurve();
        }
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
        controlPolygon.positionCount = points.Count;

        for(int i=0;i<points.Count;i++)
        {
            controlPolygon.SetPosition(i, points[i].position);
        }
    }

    void DrawBezierCurve()
    {
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
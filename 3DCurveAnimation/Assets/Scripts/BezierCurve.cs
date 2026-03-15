using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Transform p0;
    public Transform p1;
    public Transform p2;
    public Transform p3;
    private LineRenderer line;
    public int resolution = 100;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = resolution;
    }

    void Update()
    {
        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            Vector3 pos = BezierPoint(t);
            line.SetPosition(i, pos);
        }
    }

    Vector3 BezierPoint(float t)
    {
        float u = 1 - t;
        return
            u * u * u * p0.position +
            3 * u * u * t * p1.position +
            3 * u * t * t * p2.position +
            t * t * t * p3.position;
    }
}
using UnityEngine;

public class ControlPolygon : MonoBehaviour
{
    public Transform[] controlPoints;
    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        for (int i = 0; i < controlPoints.Length; i++)
        {
            line.SetPosition(i, controlPoints[i].position);
        }
    }
}
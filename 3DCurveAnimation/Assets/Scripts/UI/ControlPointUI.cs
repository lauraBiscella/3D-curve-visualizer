using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ControlPointsUIPanel : MonoBehaviour
{
    public TMP_InputField[] xInputs;
    public TMP_InputField[] yInputs;
    public TMP_InputField[] zInputs;

    [SerializeField] private CurveSystem curveSystem;

    void Update()
    {
        if (!curveSystem.HasFourElements()) return;

        for(int i=0;i<4;i++)
        {
            if(curveSystem.controlPoints[i] == null)
                continue;

            Vector3 p = curveSystem.controlPoints[i].position;

            xInputs[i].text = p.x.ToString("F2");
            yInputs[i].text = p.y.ToString("F2");
            zInputs[i].text = p.z.ToString("F2");
        }
    }

    public void OnValueChanged(int pointIndex)
    {
        if(curveSystem.controlPoints[pointIndex] == null)
            return;

        float x = float.Parse(xInputs[pointIndex].text);
        float y = float.Parse(yInputs[pointIndex].text);
        float z = float.Parse(zInputs[pointIndex].text);

        curveSystem.controlPoints[pointIndex].position =
            new Vector3(x,y,z);
    }
}
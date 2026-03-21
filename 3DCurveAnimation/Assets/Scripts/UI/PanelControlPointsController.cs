using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace BezierCurves
{
    public class ControlPointsUIPanel : MonoBehaviour
    {
        [SerializeField] private CurveSystem curveSystem;
        [SerializeField] private TMP_InputField[] xInputs;
        [SerializeField] private TMP_InputField[] yInputs;
        [SerializeField] private TMP_InputField[] zInputs;
        private bool[] inputActive = new bool[4];

        void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                int index = i;

                xInputs[i].onSelect.AddListener((s) => inputActive[index] = true);
                yInputs[i].onSelect.AddListener((s) => inputActive[index] = true);
                zInputs[i].onSelect.AddListener((s) => inputActive[index] = true);

                xInputs[i].onEndEdit.AddListener((s) => OnValueChanged(index));
                yInputs[i].onEndEdit.AddListener((s) => OnValueChanged(index));
                zInputs[i].onEndEdit.AddListener((s) => OnValueChanged(index));
            }
        }
        void Update()
        {
            if (!curveSystem.HasFourElements()) return;

            for(int i=0;i<4;i++)
            {
                if(curveSystem.controlPoints[i] == null)
                    continue;
                if (!inputActive[i])
                {
                    Vector3 p = curveSystem.controlPoints[i].position;
                    xInputs[i].text = p.x.ToString("F2");
                    yInputs[i].text = p.y.ToString("F2");
                    zInputs[i].text = p.z.ToString("F2");   
                }
            }
        }

        public void OnValueChanged(int pointIndex)
        {
            if(curveSystem.controlPoints[pointIndex] == null)
                return;

            float x = float.Parse(xInputs[pointIndex].text);
            float y = float.Parse(yInputs[pointIndex].text);
            float z = float.Parse(zInputs[pointIndex].text);

            curveSystem.controlPoints[pointIndex].position = new Vector3(x,y,z);
            
            curveSystem.SetCurveDirty();

            inputActive[pointIndex] = false;
        }
    }
}
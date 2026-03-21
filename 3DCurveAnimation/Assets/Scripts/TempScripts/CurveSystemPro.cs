using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BezierCurves
{
    public class CurveSystem : MonoBehaviour
    {
        [Header("Rendering")]
        [SerializeField] private CurveRenderer curveRenderer;
        [SerializeField] private GraphRenderer curvatureGraph;
        [SerializeField] private GraphRenderer torsionGraph;

        [Header("Control Points")]
        [SerializeField] private GameObject controlPointPrefab;  
        
        [Header("Curve Details")]
        [SerializeField] private int curveResolution = 100;

        [Header("UI Panels")]
        [SerializeField] private Slider tSlider;
        [SerializeField] private CurveInfoPanel infoPanel;
        [SerializeField] private GameObject pointIndicatorPrefab;
        public List<Transform> controlPoints = new List<Transform>();
        private List<float> curvatureValues = new List<float>();
        private List<float> torsionValues = new List<float>();
        private GameObject pointIndicator;
        private bool curveDirty = true;

        void Start()
        {
            pointIndicator = Instantiate(pointIndicatorPrefab, new Vector3(-300, -300, 0), Quaternion.identity);
            tSlider.onValueChanged.AddListener(OnSliderChanged);
        }
        void Update()
        {
            HandleMouseClick();

            if(!HasFourElements()) return;
            if (curveDirty)
            {
                curveRenderer.DrawControlPolygon(controlPoints);
                curveRenderer.DrawBezierCurve(controlPoints, curveResolution);
                DrawAnalytics();

                curveDirty = false;
            }
            RefreshSliderPosition();
        }

        public bool HasFourElements()
        {
            return controlPoints.Count == 4;
        }   

        public void SetCurveDirty()
        {
            curveDirty = true;
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
                    DraggablePoint drag = p.GetComponent<DraggablePoint>();
                    drag.SetCurveSystem(this);
                    controlPoints.Add(p.transform);

                    curveDirty = true;
                }
            }
        }

        void RefreshSliderPosition()
        {
            if (!HasFourElements()) return;

            float t = tSlider.value;
            OnSliderChanged(t);
        }
        void OnSliderChanged(float t)
        {
            if (!HasFourElements()) return;
            Vector3 bezierPos = BezierMath.BezierPoint(t, controlPoints);
            pointIndicator.transform.position = bezierPos;

            if (curvatureGraph != null)
                curvatureGraph.SetMarkerNormalized(t); 
            if (torsionGraph != null)
                torsionGraph.SetMarkerNormalized(t); 
            if (infoPanel != null)
                infoPanel.UpdateInfo(t);
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
                Vector3 d1 = BezierMath.FirstDerivative(t, controlPoints);
                Vector3 d2 = BezierMath.SecondDerivative(t, controlPoints);
                Vector3 d3 = BezierMath.ThirdDerivative(t, controlPoints);
                curvature = BezierAnalytics.Curvature(d1, d2);
                torsion = BezierAnalytics.Torsion(d1, d2, d3);
                curvatureValues.Add(curvature);
                torsionValues.Add(torsion);
            }
        }

    }
}
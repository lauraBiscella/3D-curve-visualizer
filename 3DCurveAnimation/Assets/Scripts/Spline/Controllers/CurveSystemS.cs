using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SplineCurves
{
    public class CurveSystem : MonoBehaviour
    {
        [Header("Rendering")]
        [SerializeField] private CurveRenderer curveRenderer;
        [SerializeField] private GraphRenderer curvatureGraph;
        [SerializeField] private GraphRenderer torsionGraph;

        [Header("De Boor Points")]
        [SerializeField] private GameObject deBoorPointPrefab;  
        [SerializeField] private LayerMask controlPointLayer; 
        [SerializeField] private int requiredPoints = 7;
        
        [Header("Curve Details")]
        [SerializeField] private int curveResolution = 400;
        [SerializeField] private int degree = 3;
        [SerializeField] private int bezierCount = 4;

        [Header("Knot Sliders")]
        [SerializeField] private Slider u3Slider;
        [SerializeField] private Slider u4Slider;
        [SerializeField] private Slider u5Slider;
        [SerializeField] private Slider u6Slider;
        [SerializeField] private Slider u7Slider;

        [Header("UI Panels")]
        [SerializeField] private Slider tSlider;
        [SerializeField] private CurveInfoPanel infoPanel;
        [SerializeField] private GameObject pointIndicatorPrefab;
        public float[] knots = new float[11];
        public List<Transform> deBoorPoints = new List<Transform>();
        private List<float> curvatureValues = new List<float>();
        //private List<RectTransform> nodeMarkers = new List<RectTransform>();
        private List<float> torsionValues = new List<float>();
        private GameObject pointIndicator;
        private bool curveDirty = true;
        [SerializeField] private RectTransform markerContainer;
        [SerializeField] private GameObject knotMarkerPrefab;

        private List<RectTransform> knotMarkers = new List<RectTransform>();

        void Awake()
        {
            knots = BSplineMath.GenerateKnots(bezierCount);
        }

        void Start()
        {
            pointIndicator = Instantiate(pointIndicatorPrefab, new Vector3(-300, -300, 0), Quaternion.identity);
            tSlider.onValueChanged.AddListener(OnSliderChanged);
            CreateKnotMarkers();
            UpdateKnotMarkers();
        }
        void Update()
        {
            HandleMouseClick();

            if(!HasEnoughElements()) return;
            if (curveDirty)
            {
                curveRenderer.DrawDeBoorPolygon(deBoorPoints);
                curveRenderer.DrawControlPolygon(deBoorPoints, knots);
                curveRenderer.DrawBSpline(deBoorPoints, knots, curveResolution, degree);
                //DrawAnalytics();

                curveDirty = false;
            }
            RefreshSliderPosition();
        }

        public bool HasEnoughElements()
        {
            return deBoorPoints.Count == requiredPoints;
        }   

        public void SetCurveDirty()
        {
            curveDirty = true;
        }

        void HandleMouseClick()
        {
            if (Input.GetMouseButtonDown(0) && deBoorPoints.Count < requiredPoints)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, controlPointLayer)) return;

                Plane plane = new Plane(-Camera.main.transform.forward,
                                Camera.main.transform.position + Camera.main.transform.forward * 10f);
                float distance;

                if (plane.Raycast(ray, out distance))
                {
                    Vector3 point = ray.GetPoint(distance);
                    GameObject p = Instantiate(deBoorPointPrefab, point, Quaternion.identity);
                    DraggablePoint drag = p.GetComponent<DraggablePoint>();
                    drag.SetCurveSystem(this);
                    deBoorPoints.Add(p.transform);
                    curveDirty = true;
                }
            }
        }

        public void OnKnotChanged(int index, float value)
        {
            // Primo, vincoliamo il valore per rispettare l'ordine
            float eps = 0.001f;

            if (index == 3) value = Mathf.Min(value, knots[4] - eps);
            if (index == 4) value = Mathf.Clamp(value, knots[3] + eps, knots[5] - eps);
            if (index == 5) value = Mathf.Clamp(value, knots[4] + eps, knots[6] - eps);
            if (index == 6) value = Mathf.Clamp(value, knots[5] + eps, knots[7] - eps);
            if (index == 7) value = Mathf.Max(value, knots[6] + eps);

            // Aggiorniamo il knots
            knots[index] = value;

            // Aggiorniamo i bordi ripetuti
            if (index == 3) knots[0] = knots[1] = knots[2] = knots[3];
            if (index == 7) knots[8] = knots[9] = knots[10] = knots[7];

            SetCurveDirty();
            UpdateKnotMarkers();
        }

        public float GetPreviousKnot(int index)
        {
            if (index == 3) return 0f;           // u3 non ha precedente reale
            return knots[index - 1];
        }

        public float GetNextKnot(int index)
        {
            if (index == 7) return 4f;           // u7 non ha successivo reale
            return knots[index + 1];
        }

        void RefreshSliderPosition()
        {
            if (!HasEnoughElements()) return;

            float t = tSlider.value;
            OnSliderChanged(t);
        }
        void OnSliderChanged(float t)
        {
            if (!HasEnoughElements()) return;
            
            int index = Mathf.RoundToInt(t * (curveResolution - 1));
            float sampledT = index / (float)(curveResolution - 1);

            Vector3 bSplinePos = BSplineMath.BSplinePoint(sampledT, deBoorPoints, knots, degree);
            pointIndicator.transform.position = bSplinePos;

            if (curvatureGraph != null)
                curvatureGraph.SetMarkerNormalized(sampledT); 
            if (torsionGraph != null)
                torsionGraph.SetMarkerNormalized(sampledT); 
            if (infoPanel != null)
                infoPanel.UpdateInfo(sampledT);
        }

        void CreateKnotMarkers()
        {
            for(int i = 3; i <= 7; i++)
            {
                GameObject marker = Instantiate(knotMarkerPrefab, markerContainer);
                knotMarkers.Add(marker.GetComponent<RectTransform>());
            }
        }

        void UpdateKnotMarkers()
        {
            RectTransform sliderRect = tSlider.GetComponent<RectTransform>();

            float width = sliderRect.rect.width;
            float min = tSlider.minValue;
            float max = tSlider.maxValue;

            for(int i = 0; i < knotMarkers.Count; i++)
            {
                float knotValue = knots[i + 3];

                float normalized = (knotValue - min) / (max - min);
                float x = normalized * width;

                knotMarkers[i].anchoredPosition = new Vector2(x, 0);
            }
        }

        /*
        void RefreshSliderPosition()
        {
            if (!HasEnoughElements()) return;

            float t = tSlider.value;
            OnSliderChanged(t);
        }
        void OnSliderChanged(float t)
        {
            if (!HasEnoughElements()) return;

            int index = Mathf.RoundToInt(t * (curveResolution - 1));
            float normalizedT = index / (float)(curveResolution - 1);

            float samplet = Mathf.Lerp(
                knots[degree],
                knots[controlPoints.Count],
                normalizedT
            );

            Vector3 splinePos = BSplineMath.BSplinePoint(samplet, controlPoints, knots, degree);
            pointIndicator.transform.position = splinePos;

            for(int i = 0; i < nodeMarkers.Count; i++)
            {
                nodeMarkers[i].GetComponent<UnityEngine.UI.Image>().color =
                    (t >= knots[i] && t < knots[i + 1]) ? Color.yellow : Color.white;
            }
            if (curvatureGraph != null)
                curvatureGraph.SetMarkerNormalized(samplet); 
            if (torsionGraph != null)
                torsionGraph.SetMarkerNormalized(samplet); 
            if (infoPanel != null)
                infoPanel.UpdateInfo(samplet);
        }

        void DrawAnalytics()
        {
            ComputeAnalytics();
            curvatureGraph.DrawGraph(curvatureValues);
            torsionGraph.DrawGraph(torsionValues, true);
        }

        void ComputeAnalytics()
        {
            curvatureValues.Clear();
            torsionValues.Clear();
            float curvature = 0;
            float torsion = 0;

            for(int i=0;i<curveResolution;i++)
            {
                float normalizedT = i/(float)(curveResolution-1);

                float t = Mathf.Lerp(
                    knots[degree],
                    knots[controlPoints.Count],
                    normalizedT
                );
                Vector3 d1 = BSplineMath.FirstDerivative(t, controlPoints, knots, degree);
                Debug.Log(controlPoints.Count);
                Vector3 d2 = BSplineMath.SecondDerivative(t, controlPoints, knots, degree);
                Vector3 d3 = BSplineMath.ThirdDerivative(t, controlPoints, knots, degree);
                curvature = BSplineAnalytics.Curvature(d1, d2);
                torsion = BSplineAnalytics.Torsion(d1, d2, d3);
                curvatureValues.Add(curvature);
                torsionValues.Add(torsion);
            }
        }*/

    }
}
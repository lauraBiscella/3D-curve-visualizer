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
        private bool controlPolygonVisible = false;

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

        [Header("Osculating Plane")]
        [SerializeField] private GameObject osculatingPlanePrefab;
        private GameObject osculatingPlaneInstance;
        private bool planeVisible = false;

        [Header("Frenet Frame")]
        [SerializeField] private GameObject vectorPrefab;
        private GameObject binormalVector;  
        private bool vectorVisible = false;

        [Header("UI Panels")]
        [SerializeField] private Slider tSlider;
        [SerializeField] private CurveInfoPanel infoPanel;
        [SerializeField] private GameObject pointIndicatorPrefab;

        [Header("Slider Markers")]
        [SerializeField] private GameObject knotMarkerPrefab;
        private List<GameObject> knotMarkers = new List<GameObject>();
        public float[] knots = new float[11];
        public List<Transform> deBoorPoints = new List<Transform>();
        private List<float> curvatureValues = new List<float>();
        private List<float> torsionValues = new List<float>();
        private GameObject pointIndicator;
        private bool curveDirty = true;
        [Header("Marker Colors")]
        [SerializeField] private Color[] markerColors = new Color[5]
        {
            new Color(1f, 0f, 0f),      // rosso 255,0,0
            new Color(0f, 1f, 0f),      // verde 0,255,0
            new Color(0f, 0f, 1f),      // blu 0,0,255
            new Color(1f, 1f, 0f),      // giallo 255,255,0
            new Color(0f, 1f, 1f)       // ciano 0,255,255
        };

        void Awake()
        {
            knots = BSplineMath.GenerateKnots(bezierCount);
        }

        void Start()
        {
            curveRenderer.SetControlPolygonVisibility(controlPolygonVisible);
            pointIndicator = Instantiate(pointIndicatorPrefab, new Vector3(-300, -300, 0), Quaternion.identity);
            osculatingPlaneInstance = Instantiate(osculatingPlanePrefab, Vector3.zero, Quaternion.identity);
            binormalVector = Instantiate(vectorPrefab);
            tSlider.onValueChanged.AddListener(OnSliderChanged);
             // Creo i 5 marker dei nodi
            for(int i = 0; i < 5; i++)
            {
                GameObject marker = Instantiate(knotMarkerPrefab, tSlider.fillRect.parent);
                marker.transform.SetAsLastSibling();

                Image img = marker.GetComponent<Image>();
                if(img == null) img = marker.GetComponentInChildren<Image>();
                if(img != null && markerColors.Length > i) 
                    img.color = markerColors[i];

                knotMarkers.Add(marker);
            }

            UpdateKnotMarkers(); 
        }
        void Update()
        {
            HandleMouseClick();

            if(!HasEnoughElements()) return;
            if (curveDirty)
            {
                curveRenderer.DrawDeBoorPolygon(deBoorPoints);
                if (controlPolygonVisible)
                    curveRenderer.DrawControlPolygon(deBoorPoints, knots);
                curveRenderer.DrawBSpline(deBoorPoints, knots, curveResolution, degree);
                DrawAnalytics();

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

        public void ToggleVectorVisibility()
        {
            vectorVisible = !vectorVisible;
            planeVisible = !planeVisible;

            if (binormalVector != null)
                binormalVector.SetActive(vectorVisible);
            if (osculatingPlaneInstance != null)
                osculatingPlaneInstance.SetActive(planeVisible);
        }

        public void ToggleControlPolygonVisibility()
        {
            controlPolygonVisible = !controlPolygonVisible;

            if (curveRenderer != null)
            {
                curveRenderer.SetControlPolygonVisibility(controlPolygonVisible);
                if (controlPolygonVisible && HasEnoughElements())
                    curveDirty = true;
            }
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
            List<Vector3> points = new List<Vector3>();
            foreach (Transform tr in deBoorPoints)
                points.Add(tr.position);
            
            float tStart = knots[degree];
            float tEnd = knots[deBoorPoints.Count];

            float normalized = (tSlider.value - tStart) / (tEnd - tStart);
            int index = Mathf.RoundToInt(normalized * (curveResolution - 1));

            float tNew = Mathf.Lerp(tStart, tEnd, index / (float)(curveResolution - 1));

            Vector3 bSplinePos = BSplineMath.BSplinePoint(tNew, points, knots, degree);
            pointIndicator.transform.position = bSplinePos;
            Vector3 d1 = BSplineMath.BSplineFirstDerivative(tNew, points, knots, degree);
            Vector3 d2 = BSplineMath.BSplineSecondDerivative(tNew, points, knots, degree);

            // Tangente
            Vector3 T = d1.normalized;

            // Binormale (normale del piano)
            Vector3 B = Vector3.Cross(d1, d2).normalized;

            // Normale
            Vector3 N = Vector3.Cross(B, T).normalized;

            if (curvatureGraph != null)
                curvatureGraph.SetMarker(tNew); 
            if (torsionGraph != null)
                torsionGraph.SetMarker(tNew); 
            if (infoPanel != null)
                infoPanel.UpdateInfo(tNew);
            if (osculatingPlaneInstance != null)
            {
                osculatingPlaneInstance.transform.position = bSplinePos;
                osculatingPlaneInstance.transform.rotation =
                    Quaternion.LookRotation(T, B);
            }
            if (binormalVector != null)
            {
                float scale = 0.2f;
                binormalVector.transform.position = bSplinePos;
                binormalVector.transform.rotation = Quaternion.FromToRotation(Vector3.up, B);
                binormalVector.transform.localScale = new Vector3(1, scale, 1);
            }
        }

        void UpdateKnotMarkers()
        {
            float sliderMin = tSlider.minValue;
            float sliderMax = tSlider.maxValue;
            RectTransform sliderRect = tSlider.GetComponent<RectTransform>();

            for(int i = 0; i < knotMarkers.Count; i++)
            {
                float t = knots[i + 3]; // u3-u7
                float normalized = (t - sliderMin) / (sliderMax - sliderMin);

                Vector3 localPos = sliderRect.rect.position;
                float xPos = normalized * sliderRect.rect.width;
                knotMarkers[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0);
            }
        }
        void DrawAnalytics()
        {
            ComputeAnalytics();
            List<float> tValues = new List<float>();
            float tStart = knots[degree];
            float tEnd = knots[deBoorPoints.Count];

            for (int i = 0; i < curveResolution; i++)
            {
                float t = Mathf.Lerp(tStart, tEnd, i / (float)(curveResolution - 1));
                tValues.Add(t);
            }

            curvatureGraph.DrawGraph(curvatureValues, tValues);
            torsionGraph.DrawGraph(torsionValues, tValues, true);
        }

        void ComputeAnalytics()
        {
            curvatureValues.Clear();
            torsionValues.Clear();
            float curvature = 0;
            float torsion = 0;
            float tStart = knots[degree];
            float tEnd = knots[deBoorPoints.Count];
            List<Vector3> points = new List<Vector3>();
            foreach (Transform tr in deBoorPoints)
                points.Add(tr.position);

            for(int i=0;i<curveResolution;i++)
            {
                float t = tStart + (tEnd - tStart) * (i / (float)(curveResolution - 1));
                Vector3 d1 = BSplineMath.BSplineFirstDerivative(t, points, knots, degree);
                Vector3 d2 = BSplineMath.BSplineSecondDerivative(t, points, knots, degree);
                Vector3 d3 = BSplineMath.BSplineThirdDerivative(t, points, knots, degree);
                curvature = BSplineAnalytics.Curvature(d1, d2);
                torsion = BSplineAnalytics.Torsion(d1, d2, d3);
                curvatureValues.Add(curvature);
                torsionValues.Add(torsion);
            }
        }

    }
}
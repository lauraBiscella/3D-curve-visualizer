using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace SplineCurves
{
    public class GraphRenderer : MonoBehaviour
    {
        [SerializeField] private int width = 400;
        [SerializeField] private int height = 150;
        [SerializeField] private TextMeshProUGUI labelMax;
        [SerializeField] private TextMeshProUGUI labelMin;
        [SerializeField] private Color lineColor = Color.green;

        private RawImage graphImage;
        private Texture2D tex;
        private int markerX = -1; 
        private List<float> currentValues;
        private List<float> tValues; // t reale per ogni punto
        private float maxT = 4f;     // massimo valore di t sull'asse X

        void Awake()
        {
            graphImage = GetComponent<RawImage>();
            tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            tex.wrapMode = TextureWrapMode.Clamp;
            ClearTexture();
            graphImage.texture = tex;
        }

        /// <summary>
        /// Disegna il grafico usando i valori e i corrispondenti t reali.
        /// </summary>
        public void DrawGraph(List<float> values, List<float> tRealValues, bool allowNegativeY = false)
        {
            if (values == null || values.Count < 2 || tRealValues == null || tRealValues.Count != values.Count)
                return;

            currentValues = values;
            tValues = tRealValues;

            if (tex == null) return; 
            ClearTexture();

            float minVal = float.MaxValue;
            float maxVal = float.MinValue;
            foreach (float v in values)
            {
                if (v < minVal) minVal = v;
                if (v > maxVal) maxVal = v;
            }

            if (labelMax != null)
                labelMax.text = maxVal.ToString("F2");
            if (labelMin != null)
                labelMin.text = minVal.ToString("F2");

            float rangeY = Mathf.Max(maxVal - minVal, 0.0001f);

            for (int i = 0; i < values.Count - 1; i++)
            {
                int x0 = Mathf.RoundToInt(tValues[i] / maxT * (width - 1));
                int x1 = Mathf.RoundToInt(tValues[i + 1] / maxT * (width - 1));

                int y0 = Mathf.RoundToInt((values[i] - minVal) / rangeY * (height - 1));
                int y1 = Mathf.RoundToInt((values[i + 1] - minVal) / rangeY * (height - 1));

                DrawLine(x0, y0, x1, y1, lineColor, 1);
            }

            // Linea orizzontale y=0
            int zeroY = Mathf.RoundToInt((-minVal) / rangeY * (height - 1));
            if (zeroY >= 0 && zeroY < height)
            {
                int thick = 2;
                for (int dy = -thick / 2; dy <= thick / 2; dy++)
                {
                    int y = zeroY + dy;
                    for (int x = 0; x < width; x++)
                        tex.SetPixel(x, y, Color.white);
                }
            }

            // Marker verticale
            if (markerX >= 0 && markerX < width)
            {
                int thickness = 2;
                for (int dx = -thickness / 2; dx <= thickness / 2; dx++)
                {
                    int x = markerX + dx;
                    for (int y = 0; y < height; y++)
                        tex.SetPixel(x, y, Color.red);
                }
            }

            tex.Apply();
        }

        /// <summary>
        /// Imposta il marker verticale in base a t reale.
        /// </summary>
        public void SetMarker(float tReal)
        {
            markerX = Mathf.RoundToInt(tReal / maxT * (width - 1));

            if (currentValues != null && tValues != null)
                DrawGraph(currentValues, tValues); // ridisegna il grafico con marker aggiornato
        }

        void ClearTexture()
        {
            Color[] fill = new Color[width * height];
            Color clearColor = new Color(0,0,0,0);
            for (int i = 0; i < fill.Length; i++) fill[i] = clearColor;
            tex.SetPixels(fill);
        }

        void DrawLine(int x0, int y0, int x1, int y1, Color color, int thickness)
        {
            int dx = Mathf.Abs(x1 - x0);
            int dy = Mathf.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                for (int ox = -thickness; ox <= thickness; ox++)
                    for (int oy = -thickness; oy <= thickness; oy++)
                        if (x0 + ox >= 0 && x0 + ox < width && y0 + oy >= 0 && y0 + oy < height)
                            tex.SetPixel(x0 + ox, y0 + oy, color);

                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx; y0 += sy; }
            }
        }
    }
}
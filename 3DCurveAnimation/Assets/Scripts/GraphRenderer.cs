using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GraphRenderer : MonoBehaviour
{
    [SerializeField] private int width = 200;
    [SerializeField] private int height = 150;
    [SerializeField] private TextMeshProUGUI labelMax;
    [SerializeField] private Color lineColor = Color.green;

    private RawImage graphImage;
    private Texture2D tex;

    void Awake()
    {
        graphImage = GetComponent<RawImage>();
        tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        ClearTexture();
        graphImage.texture = tex;
    }

    public void DrawGraph(List<float> values)
    {
        if (values == null || values.Count < 2) return;
        
        ClearTexture();

        float minVal = float.MaxValue;
        float maxVal = float.MinValue;
        foreach (float v in values)
        {
            if (v < minVal) minVal = v;
            if (v > maxVal) maxVal = v;
        }

        if (maxVal != 0)
            labelMax.text = maxVal.ToString("F2");
        else
            labelMax.text = "";

        float range = maxVal - minVal;
        if (range < 0.0001f) range = 1f;

        int count = values.Count;
        for (int i = 0; i < count - 1; i++)
        {
            float normalizedY0 = (values[i] - minVal) / range;
            float normalizedY1 = (values[i + 1] - minVal) / range;

            int x0 = Mathf.RoundToInt(i * (width - 1f) / (count - 1));
            int x1 = Mathf.RoundToInt((i + 1) * (width - 1f) / (count - 1));
            int y0 = Mathf.RoundToInt(normalizedY0 * (height - 1));
            int y1 = Mathf.RoundToInt(normalizedY1 * (height - 1));

            DrawLine(x0, y0, x1, y1, lineColor, 1);
        }

        tex.Apply();
    }

    void ClearTexture()
    {
        Color clearColor = new Color(0,0,0,0);
        Color[] fill = new Color[width * height];
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
            {
                for (int oy = -thickness; oy <= thickness; oy++)
                {
                    if (x0 + ox >= 0 && x0 + ox < width && y0 + oy >= 0 && y0 + oy < height)
                        tex.SetPixel(x0 + ox, y0 + oy, color);
                }
            }

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x0 += sx; }
            if (e2 < dx) { err += dx; y0 += sy; }
        }
    }
}
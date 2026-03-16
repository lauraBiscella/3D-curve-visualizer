using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GraphRenderer : MonoBehaviour
{
    public RawImage graphImage;
    public int width = 200;
    public int height = 150;
    public Color lineColor = Color.green;
    private Texture2D tex;

    void Awake()
    {
        tex = new Texture2D(width, height);
        tex.filterMode = FilterMode.Point;
        graphImage.texture = tex;
        ClearGraph();
    }

    public void DrawGraph(List<float> values)
    {
        ClearGraph();

        if (values.Count < 2) return;

        float maxVal = Mathf.Max(values.ToArray());
        float minVal = Mathf.Min(values.ToArray());

        for (int i = 1; i < values.Count; i++)
        {
            int x0 = Mathf.RoundToInt((i - 1) * (width - 1) / (values.Count - 1));
            int y0 = Mathf.RoundToInt((values[i - 1] - minVal) / Mathf.Max(maxVal - minVal, 0.0001f) * (height - 1));
            int x1 = Mathf.RoundToInt(i * (width - 1) / (values.Count - 1));
            int y1 = Mathf.RoundToInt((values[i] - minVal) / Mathf.Max(maxVal - minVal, 0.0001f) * (height - 1));

            DrawLine(x0, y0, x1, y1, lineColor);
        }

        tex.Apply();
    }

    void ClearGraph()
    {
        Color[] clearColors = new Color[width * height];
        for(int i=0;i<clearColors.Length;i++)
            clearColors[i] = Color.black; // sfondo nero
        tex.SetPixels(clearColors);
    }

    void DrawLine(int x0, int y0, int x1, int y1, Color col)
    {
        int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = -Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx + dy, e2;

        while (true)
        {
            if (x0 >= 0 && x0 < width && y0 >= 0 && y0 < height)
                tex.SetPixel(x0, y0, col);
            if (x0 == x1 && y0 == y1) break;
            e2 = 2 * err;
            if (e2 >= dy) { err += dy; x0 += sx; }
            if (e2 <= dx) { err += dx; y0 += sy; }
        }
    }
}
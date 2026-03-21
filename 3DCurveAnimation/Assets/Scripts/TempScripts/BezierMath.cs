using UnityEngine;
using System.Collections.Generic;
namespace BezierCurves
{
    public static class BezierMath
    {
        public static Vector3 BezierPoint(float t, List<Transform> points)
        {
            int n = points.Count - 1;
            Vector3 p = Vector3.zero;

            for (int i = 0; i <= n; i++)
            {
                float b = Bernstein(n, i, t);
                p += points[i].position * b;
            }

            return p;
        }

        static float Bernstein(int n, int i, float t)
        {
            return Binomial(n, i) * Mathf.Pow(t, i) * Mathf.Pow(1 - t, n - i);
        }

        static float Binomial(int n, int k)
        {
            float result = 1;

            for (int i = 1; i <= k; i++)
            {
                result *= n - (k - i);
                result /= i;
            }

            return result;
        }

        public static Vector3 FirstDerivative(float t, List<Transform> controlPoints)
        {
            int n = controlPoints.Count - 1;
            Vector3 sum = Vector3.zero;
            for (int i = 0; i < n; i++)
            {
                sum += (controlPoints[i + 1].position - controlPoints[i].position) * Bernstein(n - 1, i, t);
            }
            sum = sum * (float)n;
            return sum;
        }

        public static Vector3 SecondDerivative(float t, List<Transform> controlPoints)
        {
            int n = controlPoints.Count - 1;
            Vector3 sum = Vector3.zero;
            for (int i = 0; i < n - 1; i++)
            {
                sum += (controlPoints[i + 2].position - 2 * controlPoints[i + 1].position + controlPoints[i].position) * Bernstein(n - 2, i, t);
            }
            sum = sum * (float)(n * (n - 1));
            return sum;
        }

        public static Vector3 ThirdDerivative(float t, List<Transform> controlPoints)
        {
            int n = controlPoints.Count - 1;
            Vector3 sum = Vector3.zero;
            for (int i = 0; i < n - 2; i++)
            {
                sum += (controlPoints[i + 3].position - 3 * controlPoints[i + 2].position + 3 * controlPoints[i + 1].position - controlPoints[i].position) * Bernstein(n - 3, i, t);
            }
            sum = sum * (float)(n * (n - 1) * (n - 2));
            return sum;
        }
    }
}

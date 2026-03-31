using UnityEngine;
using System.Collections.Generic;

namespace SplineCurves
{
    public static class BSplineMath
    {
        public static float[] GenerateKnots(int bezierCount)
        {
            return new float[]
            {
                0,0,0,0,
                1,2,3,4,
                4,4,4
            };
        }

        static int FindSpan(float t, List<float> knots, int n, int degree)
        {
            if (t >= knots[n + 1]) return n;
            if (t <= knots[degree]) return degree;

            for (int i = degree; i <= n; i++)
                if (t >= knots[i] && t < knots[i + 1])
                    return i;

            return n;
        }

        public static void InsertKnot(
            float t,
            int degree,
            ref List<Vector3> controlPoints,
            ref List<float> knots)
        {
            int n = controlPoints.Count - 1;

            int k = FindSpan(t, knots, n, degree);

            List<Vector3> newPoints = new List<Vector3>(controlPoints);
            newPoints.Insert(k - degree + 1, Vector3.zero);

            for (int i = k - degree + 1; i <= k; i++)
            {
                float denom = knots[i + degree] - knots[i];
                float alpha = 0f;

                if (Mathf.Abs(denom) > 0.00001f)
                    alpha = (t - knots[i]) / denom;

                newPoints[i] =
                    (1 - alpha) * controlPoints[i - 1] +
                    alpha * controlPoints[i];
            }

            controlPoints = newPoints;
            knots.Insert(k + 1, t);
        }

        public static List<Vector3> BSplineToBezier(
            List<Vector3> deBoorPoints,
            float[] knotArray,
            int degree)
        {
            List<Vector3> cp = new List<Vector3>(deBoorPoints);
            List<float> knots = new List<float>(knotArray);

            float[] internalKnots = { 1, 2, 3 };

            foreach (float t in internalKnots)
            {
                InsertKnot(t, degree, ref cp, ref knots);
                InsertKnot(t, degree, ref cp, ref knots);
            }

            return cp;
        }

        public static Vector3 BSplinePoint(float t, List<Transform> deBoorPoints, float[] knots, int degree)
        {
            int n = deBoorPoints.Count - 1;
            if (t >= knots[n + 1]) return deBoorPoints[n].position;
            if (t <= knots[degree]) return deBoorPoints[0].position;
            
            int k = degree;
            for (int i = degree; i <= n; i++)
                if (t >= knots[i] && t < knots[i + 1])
                {
                    k = i;
                    break;
                }
            Vector3[] d = new Vector3[degree + 1];
            for (int j = 0; j <= degree; j++)
                d[j] = deBoorPoints[k - degree + j].position;
            for (int r = 1; r <= degree; r++)
            {
                for (int j = degree; j >= r; j--)
                {
                    int i = k - degree + j;

                    float denom = knots[i + degree - r + 1] - knots[i];
                    float alpha = 0;

                    if (denom > 0.00001f)
                        alpha = (t - knots[i]) / denom;

                    d[j] = (1 - alpha) * d[j - 1] + alpha * d[j];
                }
            }
            return d[degree];
        }

        /*public static Vector3 FirstDerivative(float t, List<Transform> cp, List<float> knots, int degree)
        {
            float dt = 0.0005f;

            return
            (BSplinePoint(t+dt,cp,knots,degree)
            -BSplinePoint(t-dt,cp,knots,degree))/(2*dt);
        }


        public static Vector3 SecondDerivative(float t,
                                               List<Transform> cp,
                                               List<float> knots,
                                               int degree)
        {
            float dt = 0.0005f;

            return
            (BSplinePoint(t+dt,cp,knots,degree)
            -2*BSplinePoint(t,cp,knots,degree)
            +BSplinePoint(t-dt,cp,knots,degree))/(dt*dt);
        }


        public static Vector3 ThirdDerivative(float t,
                                              List<Transform> cp,
                                              List<float> knots,
                                              int degree)
        {
            float dt = 0.0005f;

            return
            (BSplinePoint(t+2*dt,cp,knots,degree)
            -2*BSplinePoint(t+dt,cp,knots,degree)
            +2*BSplinePoint(t-dt,cp,knots,degree)
            -BSplinePoint(t-2*dt,cp,knots,degree))
            /(2*dt*dt*dt);
        }*/
    }
}
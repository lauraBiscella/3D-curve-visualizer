using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadBezier3()
    {
        SceneManager.LoadScene("BezierCurve3");
    }

    public void LoadBezier4()
    {
        SceneManager.LoadScene("BezierCurve4");
    }

    public void LoadSpline()
    {
        SceneManager.LoadScene("Spline");
    }
}
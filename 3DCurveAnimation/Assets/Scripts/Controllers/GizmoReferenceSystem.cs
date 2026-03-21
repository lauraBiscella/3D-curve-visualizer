using UnityEngine;
namespace BezierCurves
{
    public class OrientationGizmo : MonoBehaviour
    {
        public Transform mainCamera;

        void LateUpdate()
        {
            transform.rotation = Quaternion.Inverse(mainCamera.rotation);
        }
    }
}
using UnityEngine;

namespace SplineCurves
{
    public class CameraControllerPro : MonoBehaviour
    {
        [SerializeField] private CurveSystem curveSystem;
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField] private float verticalSpeed = 80f;
        [SerializeField] private float zoomSpeed = 10f;
        [SerializeField] private float minPitch = -20f;
        [SerializeField] private float maxPitch = 80f;
        [SerializeField] private float minRadius = 2f;
        [SerializeField] private float maxRadius = 50f;
        [SerializeField] private float followSpeed = 5f;
        [SerializeField] private int degree = 3;
        private float yaw = 0f;
        private float pitch = 20f;
        private float radius = 20f;
        private Vector3 target;

        void LateUpdate()
        {
            if (!curveSystem.HasEnoughElements()) return;

            UpdateCenter();
            HandleInput();
            UpdateCameraPosition();
        }

        void HandleInput()
        {
            float h = 0f;
            if (Input.GetKey(KeyCode.A)) h = -1f;
            if (Input.GetKey(KeyCode.D)) h = 1f;
            yaw += h * rotationSpeed * Time.deltaTime;

            float v = 0f;
            if (Input.GetKey(KeyCode.W)) v = 1f;
            if (Input.GetKey(KeyCode.S)) v = -1f;
            pitch += v * verticalSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            radius -= scroll * zoomSpeed;
            radius = Mathf.Clamp(radius, minRadius, maxRadius);
        }

        void UpdateCameraPosition()
        {
            float yawRad = Mathf.Deg2Rad * yaw;
            float pitchRad = Mathf.Deg2Rad * pitch;

            float x = radius * Mathf.Cos(pitchRad) * Mathf.Sin(yawRad);
            float y = radius * Mathf.Sin(pitchRad);
            float z = radius * Mathf.Cos(pitchRad) * Mathf.Cos(yawRad);

            Vector3 offset = new Vector3(x, y, z);
            transform.position = Vector3.Lerp(transform.position, target + offset, Time.deltaTime * followSpeed);
            transform.LookAt(target);
        }

        void UpdateCenter()
        {
            float t = (curveSystem.knots[degree] 
                    + curveSystem.knots[curveSystem.knots.Length - degree - 1]) * 0.5f;

            target = BSplineMath.BSplinePoint(t, curveSystem.deBoorPoints, curveSystem.knots, degree);
        }
    }
}
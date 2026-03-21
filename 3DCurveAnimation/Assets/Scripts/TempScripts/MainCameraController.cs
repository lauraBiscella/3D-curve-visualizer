using UnityEngine;

namespace BezierCurves
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
        private float yaw = 0f;
        private float pitch = 20f;
        private float radius = 20f;
        private Vector3 target;

        void Update()
        {
            if (!curveSystem.HasFourElements()) return;

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
            Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 offset = rot * new Vector3(0, 0, -radius);
            transform.position = target + offset;
            transform.LookAt(target);
        }

        void UpdateCenter()
        {
            target = BezierMath.BezierPoint(0.5f, curveSystem.controlPoints);
        }
    }
}
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    [SerializeField] private CurveSystem curveSystem;
    public float moveSpeed = 10f;
    public float mouseSensitivity = 3f;
    public float zoomSpeed = 10f;
    float rotationX = 0f;
    float rotationY = 0f;
    private Vector3 target;

    void Update()
    {
        if(curveSystem.controlPoints.Count < 4) return;
        UpdateCenter();
        HandleMovement();
        //HandleRotation();
        HandleZoom();
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.up * v;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void HandleRotation()
    {
        if(Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
            rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.position += transform.forward * scroll * zoomSpeed;
    }

    void UpdateCenter()
    {
        target = curveSystem.BezierPoint(0.5f, curveSystem.controlPoints.Count -1);
        transform.LookAt(target);
    }
}
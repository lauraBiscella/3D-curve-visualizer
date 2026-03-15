using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float mouseSensitivity = 3f;
    public float zoomSpeed = 10f;
    float rotationX = 0f;
    float rotationY = 0f;

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void HandleRotation()
    {
        if(Input.GetMouseButton(1)) // tasto destro
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
}
using UnityEngine;

public class TunnelPlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private float verticalRotation = 0f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        transform.Translate(
            new Vector3(x, 0, z) * moveSpeed * Time.deltaTime
        );

        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.Rotate(Vector3.up * mouseX);

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(
                verticalRotation, -80f, 80f
            );

            Camera.main.transform.localRotation =
                Quaternion.Euler(verticalRotation, 0, 0);
        }
    }
}
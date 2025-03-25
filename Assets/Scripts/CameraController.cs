using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera2D;
    public Camera camera3D;
    public Transform target;
    public Vector3 offset2D = new Vector3(0, 0, -10);
    public Vector3 offset3D = new Vector3(0, 5, -10);
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        // Update 2D camera position
        Vector3 desiredPosition2D = target.position + offset2D;
        Vector3 smoothedPosition2D = Vector3.Lerp(camera2D.transform.position, desiredPosition2D, smoothSpeed);
        camera2D.transform.position = smoothedPosition2D;

        // Update 3D camera position
        Vector3 desiredPosition3D = target.position + offset3D;
        Vector3 smoothedPosition3D = Vector3.Lerp(camera3D.transform.position, desiredPosition3D, smoothSpeed);
        camera3D.transform.position = smoothedPosition3D;
        camera3D.transform.LookAt(target);
    }
}

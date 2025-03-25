using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera camera2D;
    public Camera camera3D;
    private bool is2DView = true;

    void Start()
    {
        // Start with 2D view
        camera2D.enabled = true;
        camera3D.enabled = false;
    }

    void Update()
    {
        // Switch camera on spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCamera();
        }
    }

    void SwitchCamera()
    {
        is2DView = !is2DView;
        camera2D.enabled = is2DView;
        camera3D.enabled = !is2DView;
    }
}

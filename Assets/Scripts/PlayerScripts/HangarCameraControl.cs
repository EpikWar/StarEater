using UnityEngine;

public class HangarCameraControl : MonoBehaviour
{
    public static HangarCameraControl instance;
    
    [SerializeField] private float rotateSensitivity = 4f;
    [SerializeField] private float zoomSensitivity = 4f;
    [SerializeField] private float minZoomMagnitude = 20;
    [SerializeField] private float maxZoomMagnitude = 50;
    private float zoomLevel;
    private float zoomPosition;
    private Vector3 cameraVector;
    private Camera cameraMain;
    private RPGPlayerControl playerObj;

    [Header("Camera Yaw")] 
    [SerializeField] private GameObject cameraYaw;


    private void OnEnable()
    {
    #region Singelton

        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;

    #endregion

        cameraMain = Camera.main;
    }

    private void Start()
    {
        zoomPosition = minZoomMagnitude;
    }

    public void CameraControl()
    {
        CameraRotate();
        CameraZoom();
    }


    private void CameraRotate()
    {
        Vector3 currentRotation = transform.localEulerAngles;
        if (Input.GetMouseButton(1)) {
            currentRotation.y += Input.GetAxis("Mouse X") * rotateSensitivity;
            currentRotation.x += Input.GetAxis("Mouse Y") * rotateSensitivity;
            currentRotation.x = Mathf.Clamp(currentRotation.x, 270, 359);
        }

        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
    }

    private void CameraZoom()
    {
        zoomLevel -= Input.mouseScrollDelta.y * zoomSensitivity;
        zoomLevel = Mathf.Clamp(zoomLevel, minZoomMagnitude, maxZoomMagnitude);
        zoomPosition = Mathf.MoveTowards(zoomPosition, zoomLevel, 30 * Time.deltaTime);

        cameraMain.transform.localPosition = cameraVector.normalized * zoomPosition;
    }

    public void EnterHangarMode()
    {
        playerObj = RPGPlayerControl.instance;

        transform.position = new Vector3(transform.position.x, playerObj.GetPlayerHeight / 4);
        transform.rotation = Quaternion.Euler(-15, 0, 0);

        cameraMain.fieldOfView = 20;
        cameraYaw.transform.localRotation = Quaternion.Euler(30, 0, 0);

        cameraMain.transform.localPosition = new Vector3(-2.6f, 13, 25.3f);
        cameraMain.transform.localRotation = Quaternion.Euler(30, 180, 0);
        cameraVector = cameraMain.transform.localPosition - playerObj.transform.position;
    }

    public void ExitHangarMode()
    {
        cameraYaw.transform.localRotation = Quaternion.Euler(0, 0, 0);

        cameraMain.fieldOfView = 60;
    }
}

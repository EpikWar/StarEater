using System;
using UnityEngine;

public class RPGCameraControl : MonoBehaviour
{
    public static RPGCameraControl instance;
    
    [SerializeField] private float rotateSensitivity = 4f;
    [SerializeField] private float zoomSensitivity = 4f;
    [SerializeField] private float minZoomMagnitude = 20;
    [SerializeField] private float maxZoomMagnitude = 40;
    private float zoomLevel;
    private float zoomPosition;
    private Vector3 cameraVector = new (0, 10, -10);
    private Camera cameraMain;
    private RPGPlayerControl playerObj;

    [Header("Camera Yaw")] 
    [SerializeField] private GameObject cameraYaw;

    private void OnEnable()
    {
        #region Singelton

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        #endregion
    }

    private void Awake()
    {
        cameraMain = Camera.main;
        zoomPosition = minZoomMagnitude;
    }
    
    
    public void CameraControl()
    {
        CameraFollowPlayer();
        CameraRotate();
        CameraZoom();
    }
    
    private void CameraRotate()
    {
        Vector3 currentRotation = gameObject.transform.localEulerAngles;
        if (Input.GetMouseButton(1))
        {
            currentRotation.y += Input.GetAxis("Mouse X") * rotateSensitivity;
            currentRotation.x -= Input.GetAxis("Mouse Y") * rotateSensitivity;
            currentRotation.x = Mathf.Clamp(currentRotation.x, 330, 359);
        }
        transform.localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
    }

    private void CameraZoom()
    {
        zoomLevel -= Input.mouseScrollDelta.y * zoomSensitivity;
        zoomLevel = Mathf.Clamp(zoomLevel, minZoomMagnitude, maxZoomMagnitude);
        zoomPosition = Mathf.MoveTowards(zoomPosition, zoomLevel, 60 * Time.deltaTime);

        cameraMain.transform.localPosition = cameraVector.normalized * zoomPosition;
    }

    private void CameraFollowPlayer()
    {
        transform.position = playerObj.transform.position;
    }
    
    public void EnterRPGMode()
    {
        playerObj = RPGPlayerControl.instance;
        
        transform.position = playerObj.transform.position;
        transform.rotation = Quaternion.Euler(-15, transform.eulerAngles.y, transform.eulerAngles.z);
        
        cameraYaw.transform.localRotation = Quaternion.Euler(30, 0, 0);

        cameraMain.transform.localPosition = cameraVector.normalized * zoomPosition;
        cameraMain.transform.LookAt(playerObj.transform.position);
    }

    public void ExitRPGMode()
    {
        cameraYaw.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}


















using System;
using UnityEngine;

namespace UIScripts
{
    public class GameUserInterface : MonoBehaviour
    {
        public static GameUserInterface instance;
        
        [Header("Virtual Cursor")] 
        [SerializeField] protected float cameraSensitivity;
        [SerializeField] protected GameObject virtualCursor;
        private Vector3 virtualCursorPosition = new(Screen.width / 2, Screen.height / 2);
        private Vector3 lastPosition;
        private Camera camMain;
        private LayerMask layerGround;

        [Header("Game State")] 
        [SerializeField] private GameObject rpgMode;
        [SerializeField] private GameObject rtsMode;
        [SerializeField] private GameObject hangarMode;

        [Header("RPG Enemy Camera")] 
        [SerializeField] private GameObject enemyCamera;
        [SerializeField] private GameObject enemyWindow;
        private GameObject targetTrack;

        private RPGShipInfo _rpgShipInfo;


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
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Start()
        {
            camMain = Camera.main;
            layerGround = LayerMask.GetMask("Ground");
        }

        
        #region Entering & exiting modes
        // ReSharper disable Unity.PerformanceAnalysis
        public void EnterRPGMode()
        {
            rpgMode.SetActive(true);
            
            _rpgShipInfo = GetComponentInChildren<RPGShipInfo>();
        }
        public void ExitRPGMode()
        {
            rpgMode.SetActive(false);
        }
        
        public void EnterRTSMode()
        {
            rtsMode.SetActive(true);
        }
        public void ExitRTSMode()
        {
            rtsMode.SetActive(false);
        }
        
        public void EnterHangarMode()
        {
            hangarMode.SetActive(true);
        }
        public void ExitHangarMode()
        {
            hangarMode.SetActive(false);
        }
        #endregion

        #region RPGMode
        
        public void ShipInfoControl()
        {
            _rpgShipInfo.AfterburnerSlider();
        }
        
        public void LockTarget()
        {
            Ray ray = camMain.ScreenPointToRay(virtualCursor.transform.position);

            if (Input.GetKeyDown("t") || Input.GetMouseButtonDown(2))
            {
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity) && hit.collider.tag.Equals("Enemy"))
                    targetTrack = hit.collider.gameObject;
                else if (Physics.SphereCast(ray, 2f, out hit, Mathf.Infinity, ~layerGround) && hit.collider.tag.Equals("Enemy"))
                    targetTrack = hit.collider.gameObject;
            }

            try
            {
                enemyWindow.SetActive(true);

                enemyCamera.transform.position = targetTrack.transform.position;
                Vector3 position = RPGPlayerControl.instance.transform.position;
                var vLookAt = new Vector3(position.x, enemyCamera.transform.position.y, position.z);
                enemyCamera.transform.LookAt(vLookAt);
            }
            catch
            {
                enemyWindow.SetActive(false);
            }
        }

        public void RPGVirtualCursor()
        {
            virtualCursorPosition.x += Input.GetAxis("Mouse X") * cameraSensitivity;
            virtualCursorPosition.y += Input.GetAxis("Mouse Y") * cameraSensitivity;
            virtualCursorPosition.x = Math.Clamp(virtualCursorPosition.x, 0, Screen.width);
            virtualCursorPosition.y = Math.Clamp(virtualCursorPosition.y, 0, Screen.height);
            virtualCursor.transform.position = virtualCursorPosition;

            if (Input.GetMouseButtonDown(1))
                lastPosition = virtualCursor.transform.position;

            if (Input.GetMouseButton(1))
            {
                virtualCursorPosition = lastPosition;
                virtualCursor.SetActive(false);
            }
            else
            {
                virtualCursor.SetActive(true);
            }
        }

        public GameObject GetLockTarget()
        {
            return targetTrack;
        }

        public int GetLockTargetHP()
        {
            targetTrack.TryGetComponent(out EnemyControl enemyControl);
            return (int)enemyControl.CurrentHealPoint;
        }
        
        #endregion
        
        public Vector3 GetVirtualCursorPosition()
        {
            return virtualCursorPosition;
        }
    }
}
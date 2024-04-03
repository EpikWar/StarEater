using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIScripts
{
    public class GameUserInterface : MonoBehaviour
    {
        public static GameUserInterface instance;
        
        [Header("Virtual Cursor")]
        [SerializeField] protected GameObject virtualCursor;
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

        [Header("Scene Changer")] 
        [SerializeField] private GameObject rpgEnterHangar;
        [SerializeField] private GameObject rtsEnterHangar;
        
        private RPGShipInfo _rpgShipInfo;


        private void OnEnable()
        {
        #region Singelton

            if (instance != null) {
                Destroy(gameObject);
                return;
            }

            instance = this;

        #endregion
        }

        private void Start()
        {
            camMain = Camera.main;
            layerGround = LayerMask.GetMask("Ground");

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        

    #region Scene Changer

        public void EnterHangar(bool stayInTrigger)
        {
            if (GameManager.instance.IsRPGState())
                rpgEnterHangar.SetActive(stayInTrigger);

            if (GameManager.instance.IsRTSState())
                rtsEnterHangar.SetActive(stayInTrigger);
        }

        public void ExitHangar()
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single); // TODO make it changeable
        }

    #endregion

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

        public void LockTarget()
        {
            if (Input.GetKeyDown("t") || Input.GetMouseButtonDown(2)) {
                Ray ray = camMain.ScreenPointToRay(virtualCursor.transform.position);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) && hit.collider.tag.Equals("Enemy"))
                    targetTrack = hit.collider.gameObject;
                else if (Physics.SphereCast(ray, 2f, out hit, Mathf.Infinity, ~layerGround) &&
                         hit.collider.tag.Equals("Enemy"))
                    targetTrack = hit.collider.gameObject;
            }

            try {
                enemyWindow.SetActive(true);

                enemyCamera.transform.position = targetTrack.transform.position;
                Vector3 position = RPGPlayerControl.instance.transform.position;
                Vector3 vLookAt = new(position.x, enemyCamera.transform.position.y, position.z);
                enemyCamera.transform.LookAt(vLookAt);
            }
            catch {
                enemyWindow.SetActive(false);
            }
        }

        public void ShipInfoControl()
        {
            _rpgShipInfo.AfterburnerSlider();
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
            return virtualCursor.transform.position;
        }
    }
}
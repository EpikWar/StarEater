using UIScripts;
using UnityEngine;

public class RPGPlayerControl : MonoBehaviour
{
    public static RPGPlayerControl instance;

    [Header("Character properties")] 
    [SerializeField] private Stat moveSpeed;
    [SerializeField] private Stat afterburnerScale;
    [SerializeField] private Stat afterburnerMaxValue;
    [SerializeField] private Stat afterburnerDrawValue;
    [SerializeField] private Stat afterburnerChargeValue;
    private bool afterburnerCharge = true;
    private float afterburnerCurrentValue;

    [Header("Movement properties")] 
    [SerializeField] private float playerHeight;
    [SerializeField] private float playerRadius;
    [SerializeField] private Stat drag;
    private Rigidbody _rigidbody;

    [Header("RotateToCursor")] 
    [SerializeField] private float rotateSensitivity = 4f;
    [SerializeField] private GameObject playerModel;
    private Camera camMain;
    private LayerMask layerGround;

    
    #region properties

    public float GetAfterburnerMaxValue => afterburnerMaxValue.GetValue();
    public float GetAfterburnerValue => afterburnerCurrentValue;
    public float GetPlayerHeight => playerHeight;

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        print("Player Triggered by " + other.gameObject.name);
    }

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
        _rigidbody = gameObject.GetComponent<Rigidbody>();

        camMain = Camera.main;
        layerGround = LayerMask.GetMask("Ground");

        afterburnerScale.SetDefaultValue(3);
        afterburnerDrawValue.SetDefaultValue(2);
        afterburnerChargeValue.SetDefaultValue(0.2f);
        drag.SetDefaultValue(2);

        afterburnerCurrentValue = afterburnerMaxValue.GetValue();
    }

    
    public void PlayerRotation()
    {
        Vector3 currentRotation = transform.localEulerAngles;

        if (Input.GetMouseButton(1))
            currentRotation.y += Input.GetAxis("Mouse X") * rotateSensitivity;
        
        transform.rotation = Quaternion.AngleAxis(currentRotation.y, Vector3.up);
    }

    public void PlayerMovement()
    {
        Vector3 moveVector = transform.forward * Input.GetAxis("Vertical") +
                             transform.right * Input.GetAxis("Horizontal");

        if (GroundCheck()) {
            _rigidbody.drag = drag.GetValue();
        }
        else {
            _rigidbody.drag = 0;
            _rigidbody.AddForce(Vector3.down * 30, ForceMode.Force);
        }

        //Character move
        if (Input.GetKey(KeyCode.LeftShift) && afterburnerCurrentValue > 95 * (afterburnerMaxValue.GetValue() / 100) &&
            GroundCheck()) {
            afterburnerCharge = false;
        }

        if (afterburnerCharge == false && GroundCheck()) 
        {
            afterburnerCurrentValue -= afterburnerDrawValue.GetValue();
            _rigidbody.AddForce(moveVector.normalized * (moveSpeed.GetValue() * afterburnerScale.GetValue()), 
                ForceMode.Force);
            _rigidbody.drag /= 2;
        }
        else if (GroundCheck()) {
            _rigidbody.AddForce(moveVector.normalized * moveSpeed.GetValue(), ForceMode.Force);
        }

        if (afterburnerCharge == false && !GroundCheck())
            afterburnerCharge = true;

        if (afterburnerCharge == false && afterburnerCurrentValue <= 0)
            afterburnerCharge = true;

        if (afterburnerCurrentValue < afterburnerMaxValue.GetValue() && afterburnerCharge)
            afterburnerCurrentValue += afterburnerChargeValue.GetValue();
    }

    private bool GroundCheck()
    {
        LayerMask ground = LayerMask.GetMask("Ground");
        Ray ray = new(transform.position + new Vector3(0, playerRadius, 0), Vector3.down);

        return Physics.SphereCast(ray, playerRadius, playerHeight / 2 + 0.3f, ground);
    }

    public void RotateToCursor() //There was a bug, but it disappeared
    {
        Ray ray = camMain.ScreenPointToRay(GameUserInterface.instance.GetVirtualCursorPosition());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerGround)
            && !Input.GetMouseButton(1) && GameManager.instance.IsRPGState()) 
        {
            Vector3 pointRotation = new(hit.point.x, playerModel.transform.position.y, hit.point.z);
            playerModel.transform.LookAt(pointRotation);
        }
    }
}
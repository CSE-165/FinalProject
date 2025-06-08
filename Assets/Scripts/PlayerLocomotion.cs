using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField]private InputActionProperty moveReference;
    [SerializeField]private InputActionProperty snapReference;
    //[SerializeField]private InputActionAsset playerMap;
    [SerializeField]private GameObject cameraObj;
    private Vector2 moveValue;
    private Vector2 snapValue;
    private Rigidbody rb;
    [SerializeField]private float moveForce = 40f;
    [SerializeField]private float snapForce = 15f;

    void Awake()
    {
        moveReference.action.Enable();
        snapReference.action.Enable();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 6f;
        rb.angularDrag = 10f;
    }

    void Update()
    {        
        moveValue = moveReference.action.ReadValue<Vector2>();
        snapValue = snapReference.action.ReadValue<Vector2>();        
    }
    void FixedUpdate()
    {
        Vector3 moveDir = cameraObj.transform.forward * moveValue.y + cameraObj.transform.right * moveValue.x;  //combined movt direction, moveValue is y & x values, positive or negative from joystick
        rb.AddForce(moveDir*moveForce, ForceMode.Impulse);

        //Rotation
        if(snapValue.x>0f){ 
            Debug.Log("Rotation stick moved");
            rb.AddTorque(Vector3.up*snapForce, ForceMode.Impulse);
        }
        
        else if (snapValue.x < 0f)
        {
            Debug.Log("Rotation stick moved");
            rb.AddTorque(Vector3.up * -snapForce, ForceMode.Impulse);
        }
    }
}
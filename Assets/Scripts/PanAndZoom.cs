using UnityEngine;
using Cinemachine;

public class PanAndZoom : MonoBehaviour
{
     [Header("Movement Settings")]
    public float moveSpeed;
     [Header("Zoom Settings")]
     [SerializeField]
     private float zoomSpeed = 2f;
     [SerializeField]
     private float zoomInMax = 40f;
     [SerializeField]
     private float zoomOutMax = 90f;
    
    private CinemachineInputProvider inputProvider;
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private Collider boundry;
    public Vector3 targetPosition;
    // Start is called before the first frame update
private void Awake() 
{
    inputProvider = GetComponent<CinemachineInputProvider>();
    virtualCamera = GetComponent<CinemachineVirtualCamera>();
}
  
   
    public void PanScreen()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

       /* if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }*/
        targetPosition = moveDirection * moveSpeed + transform.position;
        targetPosition.x = Mathf.Clamp(targetPosition.x, boundry.bounds.min.x, boundry.bounds.max.x);
        targetPosition.z = Mathf.Clamp(targetPosition.z, boundry.bounds.min.z, boundry.bounds.max.z);

        transform.position = Vector3.Lerp(transform.position,targetPosition,Time.deltaTime);
    }

    public void ZoomScreen(float increment)
    {
        float fov = virtualCamera.m_Lens.FieldOfView;
        float target = Mathf.Clamp(fov - increment,zoomInMax, zoomOutMax);
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fov,target, zoomSpeed*Time.deltaTime);
    }
    void Update()
    {
        PanScreen();
        float z = inputProvider.GetAxisValue(2);
        if(z!=0){
            ZoomScreen(z);
        }
    }
}

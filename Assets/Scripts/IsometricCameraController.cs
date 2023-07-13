using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float edgeBoundary = 20f;
    public Vector2 minPosition;
    public Vector2 maxPosition;
    public float minZoomDistance = 10f;
    public float maxZoomDistance = 40f;

    [Header("Zoom Settings")]
    public float zoomSpeed = 20f;
    public float zoomSensitivity = 2f;
    public int zoomTicks = 0;
    public int AnoyingTracker = 0;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        // Move camera with arrow keys
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

       /*
        // Move camera with mouse
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        if (mouseX < edgeBoundary && transform.position.x > minPosition.x)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        else if (mouseX > Screen.width - edgeBoundary && transform.position.x < maxPosition.x)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        if (mouseY < edgeBoundary && transform.position.z > minPosition.y)
        {
            transform.position += Vector3.back * moveSpeed * Time.deltaTime;
        }
        else if (mouseY > Screen.height - edgeBoundary && transform.position.z < maxPosition.y)
        {
            transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
        }
*/
        // Zoom camera with mouse scroll
        //float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float scrollInput = Input.mouseScrollDelta.y;
        float distance = Vector3.Distance(transform.position, mainCamera.transform.position);
        //if(distance!= 0){ Debug.Log("Distance: " + distance);}
         if(scrollInput!=0){Debug.Log(Input.mouseScrollDelta.y);}
         Debug.Log(transform.position.y);

        if (scrollInput > 0)
        {
            if(transform.position.y>=minZoomDistance)
            {
                Debug.Log("ZOOM IN");
                //zoomTicks++;
                float zoomAmount = -scrollInput * zoomSpeed * zoomSensitivity * Time.deltaTime;
                float newDistance = Mathf.Clamp(distance - zoomAmount, minZoomDistance, maxZoomDistance);
                Vector3 direction = (mainCamera.transform.forward);
                mainCamera.transform.position = transform.position + direction * newDistance;
            }
            
        } else if(scrollInput<0)
        {
            if(transform.position.y<=maxZoomDistance)
            {
               // zoomTicks--;
                Debug.Log("ZOOM OUT");
                float zoomAmount = scrollInput * zoomSpeed * zoomSensitivity * Time.deltaTime;
                float newDistance = Mathf.Clamp(distance - zoomAmount, minZoomDistance, maxZoomDistance);
                // if(newDistance!=0){Debug.Log("newDistance: " + newDistance);}
                Vector3 direction = (-mainCamera.transform.forward);
                mainCamera.transform.position = transform.position + direction * newDistance;
            }
            
        }
    }

    private void LateUpdate()
    {
        // Keep camera within boundaries
        float xPos = Mathf.Clamp(transform.position.x, minPosition.x, maxPosition.x);
        float zPos = Mathf.Clamp(transform.position.z, minPosition.y, maxPosition.y);

        transform.position = new Vector3(xPos, transform.position.y, zPos);
    }
}

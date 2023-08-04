using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    private Camera mainCamera;
    
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}

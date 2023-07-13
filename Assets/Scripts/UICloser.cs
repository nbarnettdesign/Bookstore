using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICloser : MonoBehaviour
{
    public UIController UIController;
    private bool canClose;

    // Update is called once per frame
    void Update()
    {
        if(canClose){
            if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && UIController.uiIsUp == true)
            {
                UIController.CloseWindows();
                canClose = false;
            }
        }
         if(Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()){
            canClose = true;
         }
        
    }
}

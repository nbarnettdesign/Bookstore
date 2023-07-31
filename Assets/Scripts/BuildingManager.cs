using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;
    public float gridSize;
    bool gridOn = true;
    [SerializeField]
    private Toggle gridToggle;
    private GameObject pendingObj;
    private RaycastHit hit;
    private Vector3 pos;
    [SerializeField]
    private LayerMask layerMask;
    public float rotateAmount;
    public bool canPlace = true;

    private void FixedUpdate() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 1000, layerMask))    
        {
            pos = hit.point;
        }
    }

    public void SelectObject(int index)
    {
        //pendingObj = Instantiate(objects[index], pos, transform.Rotation);
        pendingObj = Instantiate(objects[index]);
    }

  void RotateObject()
  {
    pendingObj.transform.Rotate(Vector3.up, rotateAmount);
  }
  void ReverseRotateObject()
  {
    pendingObj.transform.Rotate(Vector3.up, -rotateAmount);
  }

    // Update is called once per frame
    void Update()
    {
        if(pendingObj != null)
        {
            if (gridOn)
            {
                pendingObj.transform.position = new Vector3(
                    RoundToNearestGrid(pos.x),
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z));
            }
            else {pendingObj.transform.position = pos;}

            if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && canPlace)
            {
                PlaceObject();
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }
            if(Input.GetKeyDown(KeyCode.T))
            {
                ReverseRotateObject();
            }
        }
    }
    
    void PlaceObject()
    {
        pendingObj.GetComponent<CheckPlacement>().isPlaced = true;
        pendingObj = null;
    }

    public void ToggleGrid()
    {
        if(gridToggle.isOn){
            gridOn = true;
        }
        else {gridOn= false; }
    }
    float RoundToNearestGrid(float pos)
        {
            //changeable Grid System
            float xDiff = pos % gridSize;
            pos -= xDiff;
            if(xDiff > (gridSize / 2))
            {
                pos += gridSize;
            }
            return pos;
        }
}

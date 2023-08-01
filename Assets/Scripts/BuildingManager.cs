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
    public UIController uiController;
    public GameController gameController;
    private bool alreadyPlacedItem;
    private Vector3 placedItemTransform;
    private Quaternion placedItemRotation;
    public GameObject cancelButton;
    public GameObject sellButton;

    private void Start() 
    {
        uiController = FindObjectOfType<UIController>();
        gameController = FindObjectOfType<GameController>();
        alreadyPlacedItem = false;
    }

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
        else if(pendingObj == null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.GetComponent<CheckPlacement>() == true)
                        {
                            alreadyPlacedItem = true;
                            placedItemTransform = hit.collider.transform.position;
                            placedItemRotation = hit.collider.transform.rotation;
                            pendingObj = hit.collider.gameObject;
                            sellButton.SetActive(true);
                            //if click on object while in build mode, if it has checkplacement script
                            // it picks it up and makes i tthe current placement object
                        }
                 } 
            }
        }
    }
    
    void PlaceObject()
    {
        if(alreadyPlacedItem == false)
        {
            if(gameController.currentGold >= pendingObj.GetComponent<CheckPlacement>().value)
            {
                gameController.currentGold -= pendingObj.GetComponent<CheckPlacement>().value;
                uiController.UpdateGoldText(gameController.currentGold);
                pendingObj.GetComponent<CheckPlacement>().isPlaced = true;
                pendingObj = null;
            }
        }
        else if (alreadyPlacedItem == true)
        {
                pendingObj.GetComponent<CheckPlacement>().isPlaced = true;
                pendingObj = null;
                alreadyPlacedItem = false;
        }
        
        
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
        
    public void CloseBuildMode()
    {
        if(alreadyPlacedItem == true)
        {
            pendingObj.transform.position = placedItemTransform;
            pendingObj.transform.rotation = placedItemRotation;
            pendingObj = null;
            alreadyPlacedItem = false;
            sellButton.SetActive(false);
        } else
        { 
            Destroy(pendingObj);
        }
    }

    public void SellButton()
    {
        if(alreadyPlacedItem == true)
        {
            gameController.currentGold += pendingObj.GetComponent<CheckPlacement>().value;
            uiController.UpdateGoldText(gameController.currentGold);
            alreadyPlacedItem = false;
            Destroy(pendingObj);
            sellButton.SetActive(false);
        }
    }
}

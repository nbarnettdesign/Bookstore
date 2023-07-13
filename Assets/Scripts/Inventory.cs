using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public GameController gameController;
    public UIController uiController;
    public List<int> inventory;
   


    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        uiController = FindObjectOfType<UIController>();
    }

    // Update is called once per frame
    public void UpdateInventoryAmounts(int i)
    {
        inventory[i]++;
        uiController.UpdateInventoryText(inventory);
    }
    

    void OnMouseDown()
{
    uiController.CloseWindows();
    uiController.UpdateInventoryText(inventory);
    uiController.OpenInventoryStatus();
}
}

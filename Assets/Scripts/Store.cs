using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public GameController gameController;
    public UIController uiController;
    public List<int> prices;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        uiController = FindObjectOfType<UIController>();
    }

    
    public void Purchase(int number)
    {
        int itemCost = prices[number];
        if(gameController.currentGold>= itemCost)
        {
            gameController.currentGold -= itemCost;
            gameController.inventory.UpdateInventoryAmounts(number);
            uiController.UpdateGoldText(gameController.currentGold);
        }
    }
    void OnMouseDown()
    {
        uiController.CloseWindows();
        uiController.OpenStoreStatus();
    }
}

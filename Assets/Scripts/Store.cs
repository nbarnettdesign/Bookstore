using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public GameController gameController;
    public UIController uiController;
    public float spiderwebChance;
    public int spiderwebMin;
    public int spiderwebMax;
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
            gameController.inventory.UpdateInventoryAmounts(number, 1);
            uiController.UpdateGoldText(gameController.currentGold);
        }
    }

    public void AddItem(int itemNumber, int amount)
    {
        gameController.inventory.UpdateInventoryAmounts(itemNumber, amount);
    }
    void OnMouseDown()
    {
        uiController.CloseWindows();
        uiController.OpenStoreStatus();
    }

    public int SpiderwebMath()
    {
        float randNum = Random.Range(0,1);
        if(randNum>spiderwebChance)
        {
            return 0;
        }
        return Random.Range(spiderwebMin,spiderwebMax);
    }
}

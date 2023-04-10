using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI currentGold;
    public TextMeshProUGUI currentTitle;
    public TextMeshProUGUI currentCost;
    public GameObject bookStatusWindow;
    public GameObject storageStatusWindow;
    public GameObject storeStatusWindow;
    public List<TextMeshProUGUI> storePrices;
    public List<TextMeshProUGUI> inventoryAmount;
    public List<TextMeshProUGUI> inventoryNames;
    public List<TextMeshProUGUI> storeItemNames;
    public GameObject inventoryStatusWindow;
    public GameObject creationStatusWindow;
    public Storage storage;
    private GameController gameController;
    
    private void Start() {
        gameController = FindObjectOfType<GameController>();
    }

    public void UpdateItemNames(List<string> itemNames)
    {
        for (int i = 0; i < itemNames.Count && i < inventoryNames.Count && i < storeItemNames.Count; i++)
    {
        inventoryNames[i].text = itemNames[i] + ":";
        storeItemNames[i].text = itemNames[i];

    }
    }

    public void UpdateGoldText(int gold)
    {
        currentGold.text = gold.ToString();
    }

    public void UpdateStoreText(List<int> prices)
    {
        for (int i = 0; i < prices.Count && i < storePrices.Count; i++)
    {
        storePrices[i].text = prices[i] + " Gold";
    }
    }
    public void UpdateInventoryText(List<int> inventory)
    {
        for (int i = 0; i < inventory.Count && i < inventoryAmount.Count; i++)
    {
        inventoryAmount[i].text = inventory[i].ToString();
    }
    }

    public void OpenBookStatus(string title, int cost)
    {
        bookStatusWindow.SetActive(true);
        currentTitle.text = title;
        currentCost.text = cost.ToString();
    }

    public void CloseBookStatus()
    {
        bookStatusWindow.SetActive(false);
    }

    public void OpenStorageStatus()
    {
        storageStatusWindow.SetActive(true);
        currentTitle.text = "title";
        currentCost.text = "";
    }

    public void CloseStorageStatus()
    {
        
         GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("StorageStatus");
        foreach (GameObject gameObject in gameObjects)
        {
            Destroy(gameObject);
        }
        storageStatusWindow.SetActive(false);
        storage.statusOpen = false;
    }

    public void OpenStoreStatus()
    {
        storeStatusWindow.SetActive(true);
    }

    public void CloseStoreStatus()
    {
        storeStatusWindow.SetActive(false);
    }
    public void OpenInventoryStatus()
    {
        inventoryStatusWindow.SetActive(true);
    }

    public void CloseInventoryStatus()
    {
        inventoryStatusWindow.SetActive(false);
    }
    public void OpenCreationStatus()
    {
        creationStatusWindow.SetActive(true);
    }

    public void CloseCreationStatus()
    {
        creationStatusWindow.SetActive(false);
    }

    
    
    public void CloseWindows()
    {
        CloseBookStatus();
        CloseStorageStatus();
        CloseStoreStatus();
        CloseInventoryStatus();
        CloseCreationStatus();

    }

    
}

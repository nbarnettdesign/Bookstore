using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookCreationPage : MonoBehaviour
{
    public GameObject prefab;
    public Button createButton;
    public TextMeshProUGUI titleBox;
    private Inventory inventory;
    private Storage storage;
    private GameController gameController;
    public int ingredient1number;
    public TextMeshProUGUI ingredients1;
    public TextMeshProUGUI ingredientsAmount1;
    public int needed1;
    public int have1;
    public int ingredient2number;
    public TextMeshProUGUI ingredients2;
    public TextMeshProUGUI ingredientsAmount2;
    public int needed2;
    public int have2;
    public int ingredient3number;
    public TextMeshProUGUI ingredients3;
    public TextMeshProUGUI ingredientsAmount3;
    public int needed3;
    public int have3;
    public int price;
    public TextMeshProUGUI priceText;

    private bool ingr1;
    private bool ingr2;
    private bool ingr3;
    // Start is called before the first frame update
    void OnEnable()
    {
        inventory = FindObjectOfType<Inventory>();
        storage = FindObjectOfType<Storage>();
        gameController = FindObjectOfType<GameController>();
        createButton.interactable = false;
        UpdatePage();
    }

    public void UpdatePage()
    {
        have1 = inventory.inventory[ingredient1number];
        ingredients1.text = gameController.itemNames[ingredient1number];
        have2 = inventory.inventory[ingredient2number];
        ingredients2.text = gameController.itemNames[ingredient2number];
        have3 = inventory.inventory[ingredient3number];
        ingredients3.text = gameController.itemNames[ingredient3number];
        ingredientsAmount1.text = have1 + "/"+needed1;
        ingredientsAmount2.text = have2 + "/"+needed2;
        ingredientsAmount3.text = have3 + "/"+needed3;

        if(needed1 ==0)
            {
                ingredients1.text = "";
                ingredientsAmount1.text = "";
                ingr1 = true;
            }
        if(needed2 ==0)
            {
                ingredients2.text = "";
                ingredientsAmount2.text = "";
                ingr2 = true;
            }
        if(needed3 ==0)
            {
                ingredients3.text = "";
                ingredientsAmount3.text = "";
                ingr3 = true;
            }
        priceText.text = price + " Gold";
        
        if(needed1<=have1)
            {
                ingredients1.color = Color.green;
                ingredientsAmount1.color = Color.green;
                ingr1 = true;
            }else{
                ingredients1.color = Color.white;
                ingredientsAmount1.color = Color.white;
                ingr1 = false;
            }
        if(needed2<=have2)
            {
                ingredients2.color = Color.green;
                ingredientsAmount2.color = Color.green;
                ingr2 = true;
            }else{
                ingredients2.color = Color.white;
                ingredientsAmount2.color = Color.white;
                ingr2 = false;
        }
        if(needed3<=have3)
            {
                ingredients3.color = Color.green;
                ingredientsAmount3.color = Color.green;
                ingr3 = true;
            }else{
                ingredients3.color = Color.white;
                ingredientsAmount3.color = Color.white;
                ingr3 = true;
            }
        if(ingr1 && ingr2 && ingr3)
        {
            createButton.interactable = true;
        }else createButton.interactable = false;
    }

   public void CreateBook()
    {
        inventory.inventory[ingredient1number] -= needed1;
        inventory.inventory[ingredient2number] -= needed2;
        inventory.inventory[ingredient3number] -= needed3;
        string newname = titleBox.text;
        if (newname.Length == 1)
            {
                string name = "Solo Leveling Chapter " + Random.Range(1, 500);
                storage.AddBook(name, price, prefab);
                storage.booksInStorage++;
            }
            else
            {
                storage.AddBook(newname, price, prefab);
                storage.booksInStorage++;
                gameController.RestockShelves();
            }

        UpdatePage();
    }

}

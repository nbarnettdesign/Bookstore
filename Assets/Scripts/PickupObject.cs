using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public int price = 10;
    public GameObject pickedUpObject;
    public GameObject prefab;
    public BookSpot bookSpot;
    public string bookName;
    private bool isPickedUp = false;
    public bool isAvailable = true;
    private Customer customer;
    public UIController uiController;
    private GameController gameController;

    public float pickupRange = 2f; // define the pickup range


private void Start() {
    bookSpot = GetComponentInParent<BookSpot>();
    bookSpot.currentBook = this;
    bookSpot.isAvailable = false;
        isPickedUp = false;
    uiController = FindObjectOfType<UIController>();
    
    if(bookName=="")
    {
        bookName = "Solo Leveling Chapter " + Random.Range( 1,500);
    }
}
   /* Need something to pick up book here
    * when right clicked player goes over and picks up, then this book nerds out, copy the is being carried i guess
     */

    public void SetIsBeingCarried(bool isBeingCarried)
    {
        if (isBeingCarried)
        {
            isPickedUp = true;
            pickedUpObject.SetActive(false);
            bookSpot.currentBook = null;
            bookSpot.isAvailable = true;
        }
        else
        {
            isPickedUp = false;
            pickedUpObject.SetActive(true);
        }
    }
    void OnMouseDown()
{
    uiController.CloseWindows();
    uiController.OpenBookStatus(bookName, price);
}

}



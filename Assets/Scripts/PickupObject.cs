using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public int price = 10;
    public GameObject pickedUpObject;
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
    uiController = FindObjectOfType<UIController>();
    
    if(bookName=="")
    {
        bookName = "Solo Leveling Chapter " + Random.Range( 1,500);
    }
}
/*
private void OnTriggerEnter(Collider other)
{
    if (!isPickedUp && other.gameObject.CompareTag("Customer"))
    {
        customer = other.gameObject.GetComponent<Customer>();
        if (customer != null && customer.PickUpObject(this, customer.pickupRange))
        {
            Debug.Log("IS THIS BEING USED?");
            isPickedUp = true;
            pickedUpObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Could not pick up object: customer is null, already has an object, or is out of range.");
        }
    }
}
    /*public void Reset()
    {
        isPickedUp = false;
        customer = null;
        pickedUpObject.SetActive(true);
    }*/

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



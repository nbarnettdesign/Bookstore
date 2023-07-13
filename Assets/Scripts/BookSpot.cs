using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSpot : MonoBehaviour
{
    public GameObject bookGhost;
    private bool hovering;
    public PickUpObject currentBook;
    private UIController uIController;
    public bool isAvailable;
    public string bookName;
    public int price;
    public PickUpObject pickup;
    // Start is called before the first frame update
    private void Start() {
        bookGhost.SetActive(false);
        hovering = false;
        uIController = FindObjectOfType<UIController>();
    }
    void OnMouseOver() 
    {
        bookGhost.SetActive(true);    
    }

    private void OnMouseExit() 
    {
        bookGhost.SetActive(false);
    }

    private void OnMouseDown() {
        if(pickup != null)
        {
            if(isAvailable == false)
            {
                uIController.CloseWindows();
                uIController.OpenBookStatus(bookName, price);
            }

        }
        
        
    }
}

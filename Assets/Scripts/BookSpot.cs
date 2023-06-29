using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSpot : MonoBehaviour
{
    public GameObject bookGhost;
    private bool hovering;
    public PickUpObject currentBook;
    public bool isAvailable;
    // Start is called before the first frame update
    private void Start() {
        bookGhost.SetActive(false);
        hovering = false;
    }
    void OnMouseOver() 
    {
        bookGhost.SetActive(true);    
    }

    private void OnMouseExit() 
    {
        bookGhost.SetActive(false);
    }
}

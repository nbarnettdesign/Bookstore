using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [System.Serializable]
    public class Book
    {
        public string name;
        public int value;
        public GameObject prefab;
        
    }

    public List<Book> bookList = new List<Book>();
    public int booksInStorage;
    private UIController uiController;
    private GameController gameController;
    public GameObject bookUIPrefab;
    public bool statusOpen;
    public GameObject booksOnTop1;
    public GameObject booksOnTop2;
    public GameObject booksOnTop3;

    private void Start() {
        gameController = FindObjectOfType<GameController>();
        booksInStorage = bookList.Count;
        //Debug.Log("Books in storage: "+booksInStorage);
        gameController.RestockShelves();
        uiController = FindObjectOfType<UIController>();
        statusOpen = false;
        BooksOnTop();
    }

    private void Update() {
        if(booksInStorage == bookList.Count)
        {
            booksInStorage = bookList.Count;
        }
    }

    public void AddBook(string name, int value, GameObject prefab)
    {
        Book newBook = new Book();
        newBook.name = name;
        newBook.value = value;
        newBook.prefab = prefab;
        bookList.Add(newBook);
        BooksOnTop();
    }
    void OnMouseDown()
    {
        if(!statusOpen)
        {
           uiController.CloseWindows();
           uiController.OpenStorageStatus();

        // Set initial y position
        PopulateBookList();
        }

          
    }
    public void PopulateBookList()
    {
        float yPosition = 110f;
        foreach (Book book in bookList)
            {
                // Create a new instance of the book UI prefab
                GameObject bookUI = Instantiate(bookUIPrefab, uiController.storageStatusWindow.transform);

                // Set the position of the book UI
                RectTransform rectTransform = bookUI.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0f, yPosition);

                // Set the name and value of the book on the UI prefab
                bookUI.GetComponent<BookUI>().SetNameAndValue(book.name, book.value);

                // Adjust the y position for the next book UI
                yPosition -= 20f; // or whatever value you want
            }
            statusOpen = true;     
        
    }

    public void BooksOnTop()
    {
        if(booksInStorage == 0){
            booksOnTop1.SetActive(false);
            booksOnTop2.SetActive(false);
            booksOnTop3.SetActive(false);
        }
        else if(booksInStorage>0 && booksInStorage<=2)
        {
            booksOnTop1.SetActive(true);
            booksOnTop2.SetActive(false);
            booksOnTop3.SetActive(false);
        }
        else if(booksInStorage>2 && booksInStorage<=4)
        {
            booksOnTop1.SetActive(true);
            booksOnTop2.SetActive(true);
            booksOnTop3.SetActive(false);
        }
        else if(booksInStorage>4)
        {
            booksOnTop1.SetActive(true);
            booksOnTop2.SetActive(true);
            booksOnTop3.SetActive(true);
        }
    }
}

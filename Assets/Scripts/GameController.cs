using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
public int maxCustomers = 5;
public int currentCustomerCount;
public UIController UIController;
public Store store;
public BookCreation bookCreation;
public Inventory inventory;
public int books = 3;
public int currentGold; 
private int currentBooks;
public float CustomerSpawnTime;
public float CustomerSpawnTimeMin;
public float CustomerSpawnTimeMax;
public GameObject Customer;
public GameObject Door;
public GameObject employeeDoor;
public Storage storage;
public GameObject worker;
public GameObject currentWorker;
public bool restockInProgress;
public List<BookSpot> availableBookSpots;
 public List<string> itemNames;
 public List<int> prices;

public bool cashierNeeded;
public bool restockNeeded;
private float CustomerSpawnTimer;



    // Start is called before the first frame update
    void Start()
    {
        SetImportantObjects();
        CustomerSpawnTimer = 0f;
        books = 0;
        CustomerSpawnTime = Random.Range(CustomerSpawnTimeMin,CustomerSpawnTimeMax);
        BookSpot[] availableBookSpotsArray = FindObjectsOfType<BookSpot>();
        cashierNeeded = false;
        restockNeeded = false;
        UpdateItemNamesandPrices();

        foreach (BookSpot bookSpotCheck in availableBookSpotsArray)
        {
            if (bookSpotCheck.isAvailable == false)
            {
                books ++;
            }
        }
        RestockShelves();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTimer();
    }
    private void SpawnCustomer()
    {
        CustomerSpawnTimer = 0f;
        CustomerSpawnTime = Random.Range(CustomerSpawnTimeMin,CustomerSpawnTimeMax);
        currentCustomerCount++;
        GameObject instance = Instantiate(Customer, Door.transform.position, Quaternion.identity);
    }
    private void SpawnTimer()
    {
        if(maxCustomers<= books){
                    if(currentCustomerCount<maxCustomers)
                    {
                        if (CustomerSpawnTimer<=CustomerSpawnTime)
                        {
                            CustomerSpawnTimer+=Time.deltaTime;                      
                        }
                        else {
                            SpawnCustomer();
                            }
                    }
        }
        if(books < maxCustomers)
        {
            if(currentCustomerCount<books)
            {
                /*if(currentCustomerCount<books)
                {*/
                    if (CustomerSpawnTimer<=CustomerSpawnTime)
                    {
                        CustomerSpawnTimer+=Time.deltaTime;
                    }
                    else {
                        SpawnCustomer();
                    }
                //}
            }
        }
    }

    public void SetImportantObjects()
    {
        store = FindObjectOfType<Store>();
        bookCreation = FindObjectOfType<BookCreation>();
        inventory = FindObjectOfType<Inventory>();
    }
    public void GetAvailableBooks()
    {
        availableBookSpots.Clear();
        BookSpot[] availableBookSpotsArray = FindObjectsOfType<BookSpot>();

        foreach (BookSpot bookSpotCheck in availableBookSpotsArray)
        {
            if (bookSpotCheck.isAvailable)
            {
                availableBookSpots.Add(bookSpotCheck);
            }
        }
    }

    public void RestockShelves()
    {
        GetAvailableBooks();
        if (storage.booksInStorage >0 && availableBookSpots.Count >0)
            {
        restockNeeded = true;
            } else restockNeeded = false;
        /*if(!restockInProgress)
        {
            GetAvailableBooks();
            if (storage.booksInStorage >0 && availableBookSpots.Count >0)
            {
                Debug.Log("Time to Restock");
                restockInProgress = true;
                currentWorker = Instantiate(worker, employeeDoor.transform.position, Quaternion.identity);
                currentWorker.GetComponent<Worker>().availableBookSpots = availableBookSpots;
                currentWorker.GetComponent<Worker>().employeeDoor = employeeDoor.transform.position;
                currentWorker.GetComponent<Worker>().gameController = this;
            }
        }*/
        
    }

    public void ContinueStocking()
    {
        /*
            if books in storage greater than 0


            else go away, but make sure to turn restock in progress off
        */
    }

    public void UpdateItemNamesandPrices()
    {
         UIController.UpdateItemNames(itemNames);
         UIController.UpdateStoreText(prices);
    }

}

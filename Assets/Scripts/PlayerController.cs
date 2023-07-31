using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Worker;

public class PlayerController : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    private PickUpObject targetPickupObject;
    public UIController uiController;
    public Store store;
    public BookCreation bookCreation;
    public Inventory inventory;
    public Storage storage;
    public GameController gameController;
    public PaymentLine cashRegister;
    public string carriedBookName;
    public int carriedBookPrice;
    public GameObject carriedBookPrefab;
    public bool workingAtCheckout;
    public bool atCheckout;
    public bool hasbook;
    public bool workingAtStorage;
    public bool workingAtBookshelf;
    public bool takingABook;
    public bool workingAtCreation;
    public bool atCreation;
    public bool cleaning;
    BookSpot availableBookSpot;
    public int howManyBooksCanCarry;
    public List<string> carriedBookNames;
    public List<int> carriedBookPrices;
    public List<GameObject> carriedBookPrefabs;
    [SerializeField] float minCleanTime;
    [SerializeField] float maxCleanTime;
    float cleanTimer;


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        store = FindObjectOfType<Store>();
        bookCreation = FindObjectOfType<BookCreation>();
        inventory = FindObjectOfType<Inventory>();
        uiController = FindObjectOfType<UIController>();
        storage = FindObjectOfType<Storage>();
        gameController = FindObjectOfType<GameController>();
        cashRegister = FindObjectOfType<PaymentLine>(); 
        workingAtCheckout = false;
        atCheckout = false;
        hasbook = false;
        workingAtStorage = false;
        workingAtBookshelf = false;
        workingAtCreation = false;
        cleaning = false;
        cleanTimer = Random.Range(minCleanTime,maxCleanTime);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    navMeshAgent.SetDestination(hit.point);
                    
                    if(workingAtCheckout == true)
                    {
                        workingAtCheckout = false;
                        cashRegister.cashierAssigned = false;
                        cashRegister.cashierAtTill = false;
                        atCheckout = false; 
                    }
                    if (atCreation)
                    {
                        uiController.CloseCreationStatus();
                        atCreation = false;
                    }
                    workingAtBookshelf = false;
                    workingAtStorage = false;
                    workingAtCreation = false;
                    takingABook = false;
                    cleaning = false;
                }
                else if (hit.collider.CompareTag("CheckoutDesk"))
                {
                    WorkAtCheckoutDesk();
                    if (atCreation)
                    {
                        uiController.CloseCreationStatus();
                        atCreation = false;
                    }
                    workingAtBookshelf = false;
                    workingAtStorage = false;
                    workingAtCreation = false;
                    takingABook = false;
                    cleaning = false;

                }
                else if (hit.collider.CompareTag("Storage"))
                {
                    workingAtStorage = true;
                    navMeshAgent.destination = (storage.transform.position);
                    if (workingAtCheckout == true)
                    {
                        workingAtCheckout = false;
                        cashRegister.cashierAssigned = false;
                        cashRegister.cashierAtTill = false;
                        atCheckout = false;
                    }
                    if (atCreation)
                    {
                        uiController.CloseCreationStatus();
                        atCreation = false;
                    }
                    workingAtBookshelf = false;
                    workingAtCreation = false;
                    takingABook = false;
                    cleaning = false;
                }
                else if (hit.collider.CompareTag("Bookshelf"))
                {
                    PickABookshelf(hit.collider.GetComponent<Bookshelf>());
                    if (workingAtCheckout == true)
                    {
                        workingAtCheckout = false;
                        cashRegister.cashierAssigned = false;
                        cashRegister.cashierAtTill = false;
                        atCheckout = false;
                    }
                    if (atCreation)
                    {
                        uiController.CloseCreationStatus();
                        atCreation = false;
                    }
                    workingAtStorage = false;
                    workingAtCreation = false;
                    takingABook = false;
                    cleaning = false;
                }
                else if (hit.collider.CompareTag("BookPoint"))
                {
                    BookPointPicked(hit.collider.GetComponent<BookSpot>());
                    if (workingAtCheckout == true)
                    {
                        workingAtCheckout = false;
                        cashRegister.cashierAssigned = false;
                        cashRegister.cashierAtTill = false;
                        atCheckout = false;
                    }
                    if (atCreation)
                    {
                        uiController.CloseCreationStatus();
                        atCreation = false;
                    }
                    workingAtStorage = false;
                    workingAtCreation = false;
                    cleaning = false;
                    
                }
                else if (hit.collider.CompareTag("BookCreation"))
                {
                    workingAtCreation = true;
                    navMeshAgent.SetDestination(bookCreation.transform.position);
                    if (workingAtCheckout == true)
                    {
                        workingAtCheckout = false;
                        cashRegister.cashierAssigned = false;
                        cashRegister.cashierAtTill = false;
                        atCheckout = false;
                    }
                    workingAtStorage = false;
                    workingAtBookshelf = false;
                    cleaning = false;
                }
                else if (hit.collider.GetComponent<Decorations>() != null)
                {
                    if(hit.collider.GetComponent<Decorations>().cleanable == true)
                    {
                    
                        cleaning = true;
                        cleanTimer = Random.Range(minCleanTime,maxCleanTime);
                        Debug.Log("CLEAN TIME: " + cleanTimer);
                        navMeshAgent.SetDestination(hit.collider.transform.position);
                        if (workingAtCheckout == true)
                        {
                            workingAtCheckout = false;
                            cashRegister.cashierAssigned = false;
                            cashRegister.cashierAtTill = false;
                            atCheckout = false;
                        }
                        workingAtStorage = false;
                        workingAtBookshelf = false;
                        workingAtCreation = false;
                    }
                }
            }
        }

            if(workingAtCheckout == true)
            {
                if (Vector3.Distance(this.transform.position, cashRegister.cashierPoint.position) < 1f)
                    {
                        cashRegister.cashierAtTill = true;
                        FaceTarget(cashRegister.paymentPoint.transform.position);
                        atCheckout = true;
                    }
            }
            else if (workingAtStorage == true)
            {
                if (Vector3.Distance(this.transform.position, storage.transform.position) < 2f)
                {
                    WorkAtStorage();
                }
            }
            else if (workingAtBookshelf == true)
            {
                if (Vector3.Distance(this.transform.position, navMeshAgent.destination) < 2f)
                {
                    StockABookshelf();
                }
            }
            else if (workingAtCreation == true)
            {
                if (Vector3.Distance(this.transform.position, bookCreation.transform.position) < 2f)
                {
                    uiController.OpenCreationStatus();
                    workingAtCreation = false;
                    atCreation = true;
                }
            }
            else if (takingABook == true)
            {
                if (Vector3.Distance(this.transform.position, navMeshAgent.destination) < 2f)
                {
                    TakeBookOffShelf();
                }
            }
            else if (cleaning == true)
            {
                if (Vector3.Distance(this.transform.position, navMeshAgent.destination) < 2f)
                {
                    CleanSomething();
                }
            }
        }

    public void WorkAtCheckoutDesk()
    {
        workingAtCheckout = true;
        if(cashRegister.currentCashier != null)
        {
            cashRegister.currentCashier.currentState = Worker.WorkerState.Idle;
            cashRegister.currentCashier = null;
        }
        cashRegister.cashierAtTill = false;
        cashRegister.cashierAssigned = true;
        navMeshAgent.destination =(cashRegister.cashierPoint.position);
    }
    public void CleanSomething()
    {
        if(cleanTimer >0)
        {
            cleanTimer-=Time.deltaTime;
        }
        else if (cleanTimer <= 0)
        {
            int spiderwebAmount = store.SpiderwebMath();
            store.AddItem(0,spiderwebAmount);
            cleanTimer = Random.Range(minCleanTime, maxCleanTime);
            Debug.Log("Spider Web: " + spiderwebAmount);
            cleaning = false;
        }
        
    }

    public void PickABookshelf(Bookshelf shelf)
    {
        foreach (BookSpot bookspot in shelf.bookSpots)
        {
            if (bookspot.isAvailable)
            {
                availableBookSpot = bookspot;
                navMeshAgent.SetDestination(availableBookSpot.transform.position);
                workingAtBookshelf = true;
                break; // Stop searching after finding the first available BookSpot
            }
        }
        if(availableBookSpot == null)
        {
            navMeshAgent.SetDestination(shelf.transform.position);
        }
    }
    public void BookPointPicked(BookSpot bookSpot)
    {
        if (bookSpot.isAvailable)
        {
            availableBookSpot = bookSpot;
            navMeshAgent.SetDestination(availableBookSpot.transform.position);
            workingAtBookshelf = true;
            takingABook = false;
        } 
        else if(bookSpot.isAvailable == false && carriedBookNames.Count < howManyBooksCanCarry)
        {
            workingAtBookshelf = false;
            navMeshAgent.SetDestination(bookSpot.transform.position);
            targetPickupObject = bookSpot.currentBook;
            takingABook = true;
        }
    }

    public void StockABookshelf()
    {
        if(hasbook)
        {
            if (availableBookSpot != null)
            {
                FaceTarget(availableBookSpot.transform.position);
                GameObject newBook = Instantiate(carriedBookPrefabs[0], availableBookSpot.transform.position, Quaternion.identity, availableBookSpot.transform);
                newBook.GetComponent<PickUpObject>().prefab = carriedBookPrefabs[0];
                newBook.transform.localRotation = Quaternion.Euler(0, 90, 0);
                newBook.transform.localPosition = new Vector3(0, 0.23f, 0);
                newBook.GetComponent<PickUpObject>().bookName = carriedBookNames[0];
                newBook.GetComponent<PickUpObject>().price = carriedBookPrices[0];
                carriedBookNames.RemoveAt(0);
                carriedBookPrices.RemoveAt(0);
                carriedBookPrefabs.RemoveAt(0);
                gameController.books++;
                if(carriedBookNames.Count == 0)
                {
                    hasbook = false;
                } 
                workingAtBookshelf = false;
                availableBookSpot = null;
                // ... do something with the available BookSpot ...
            }
            else if (availableBookSpot == null)
            {
                Debug.Log("No available BookSpot found on the bookcase.");
            }
        }
    }
    public void TakeBookOffShelf()
    {
        if(targetPickupObject != null)
        {
            carriedBookNames.Add(targetPickupObject.bookName);
            targetPickupObject.bookName = null;
            carriedBookPrices.Add(targetPickupObject.price);
            targetPickupObject.price = 0;
            carriedBookPrefabs.Add(targetPickupObject.prefab);
            targetPickupObject.bookSpot.isAvailable = true;
            Destroy(targetPickupObject.gameObject);
            if(carriedBookNames.Count>0){hasbook = true;}
            gameController.books--;
            
            takingABook = false;
        }
        /*
        Check if target book is not null
        if not, then take info from target book, add to list (Already made sure that you have room)
        remove book from shelf
        make shelf available again

        ADD TO EVERYWHERE ELSE THAT IF YOU CLICK THEN TARGET BOOK = NULL
        */
    }
    public void WorkAtStorage()
    {
        FaceTarget(storage.transform.position);
        if (storage.GetComponent<Storage>().bookList.Count > 0)
        {
            if(carriedBookNames.Count < howManyBooksCanCarry)
            {
                carriedBookNames.Add(storage.GetComponent<Storage>().bookList[0].name);
                carriedBookPrices.Add(storage.GetComponent<Storage>().bookList[0].value);
                carriedBookPrefabs.Add(storage.GetComponent<Storage>().bookList[0].prefab);
                storage.GetComponent<Storage>().bookList.RemoveAt(0);
                storage.GetComponent<Storage>().booksInStorage--;
                uiController.DeleteStorageList();
                storage.GetComponent<Storage>().PopulateBookList();
                storage.GetComponent<Storage>().BooksOnTop();
                hasbook = true;
            }
            /*  @@@ bellow is for putting books back into storage, need a different clickable to do that (Maybe Button in Storage)
            else if (carriedBookNames.Count >= howManyBooksCanCarry)
            {
                storage.AddBook(carriedBookName, carriedBookPrice, carriedBookPrefab);
                storage.GetComponent<Storage>().booksInStorage++;
                carriedBookName = null;
                carriedBookPrice = 0;
                carriedBookPrefab = null;
                hasbook = false;
            }
            */
            workingAtStorage = false;
            navMeshAgent.SetDestination(this.transform.position);
        }
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);  
    }

    public void AddBookToList(string bookName, int bookPrice, GameObject bookPrefab)
    {
        carriedBookNames.Add(bookName);
        carriedBookPrices.Add(bookPrice);
        carriedBookPrefabs.Add(bookPrefab);
    }
}

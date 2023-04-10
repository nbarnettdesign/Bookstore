using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    // Possible states for the worker
    public enum WorkerState { Idle, Cashier, RetrieveBooksFromStorage, StockShelves, Exit, Cleaning }

    // Current state of the worker
    public WorkerState currentState;

    // Reference to the NavMeshAgent component
    private NavMeshAgent navMeshAgent;

    // Reference to the cashier object
    public GameObject cashier;

    public string carriedBookName;
    public int carriedBookPrice;
    public GameObject carriedBookPrefab;
    private GameObject storage;
    public BookSpot bookSpotFree;
    public List<BookSpot> availableBookSpots;
    public Vector3 employeeDoor;
    public GameController gameController;
    public PaymentLine cashRegister;
    public float workTime;
    private float workTimer;
    private bool cleaning;
    private int randomIndex;
    private int randomIndexCleaning;
    private bool goingToRegister;
    private List<Decorations> decorationToClean;
    private Decorations cleanTarget;
    private UIController uiController;

    // Start is called before the first frame update
    void Start()
    {
        // Get reference to the NavMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();
        uiController = FindObjectOfType<UIController>();
        storage = GameObject.FindGameObjectWithTag("Storage");
        currentState = WorkerState.Idle;
        workTimer =0f;
        goingToRegister = false;

        decorationToClean = new List<Decorations>();
    
        Decorations[] allDecorations = FindObjectsOfType<Decorations>();
    
        foreach (Decorations decoration in allDecorations)
        {
            if (decoration.cleanable)
            {
                decorationToClean.Add(decoration);
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case WorkerState.Idle:
                if(cashRegister.cashierAssigned == false && gameController.cashierNeeded == true)
                    {
                        currentState = WorkerState.Cashier;
                        cashRegister.cashierAssigned = true;
                        goingToRegister = true;
                    }
                else if(gameController.restockNeeded == true && goingToRegister == false  && gameController.restockInProgress == false)  
                {
                    currentState = WorkerState.RetrieveBooksFromStorage;
                    gameController.restockInProgress = true;
                } 
                else currentState = WorkerState.Cleaning;
                /*if(gameController.cashierNeeded == false || cashRegister.cashierAssigned == true)
                {
                    if(gameController.cashierNeeded == false || cashRegister.cashierAssigned == true)
                    {
                        if(gameController.restockNeeded == false || gameController.restockInProgress == true)
                        {
                            currentState = WorkerState.Cleaning;
                        }
                    }
                }*/
                
                break;

            case WorkerState.Cashier:
                navMeshAgent.destination =(cashRegister.cashierPoint.position);
               if (Vector3.Distance(this.transform.position, cashRegister.cashierPoint.position) < 1f)
                {
                    cashRegister.cashierAtTill = true;
                    FaceTarget(cashRegister.paymentPoint.transform.position);
                    cashRegister.currentCashier = this;
                }
                if(gameController.cashierNeeded == false)
                {
                    currentState = WorkerState.Idle;
                    cashRegister.cashierAssigned = false;
                    cashRegister.cashierAtTill = false;
                    goingToRegister = false;
                    if(cashRegister.currentCashier == this)
                    {
                        cashRegister.currentCashier = null;
                    }
                }
                break;

            case WorkerState.RetrieveBooksFromStorage:
                // Set destination to the stock shelves object
                 navMeshAgent.destination =(storage.transform.position);
                if (Vector3.Distance(this.transform.position, storage.transform.position) < 2f)
                {
                    FaceTarget(storage.transform.position);

                    if(workTimer<=workTime)
                    {
                        workTimer+=Time.deltaTime;
                    }
                    else{
                        carriedBookName = storage.GetComponent<Storage>().bookList[0].name;
                        carriedBookPrice = storage.GetComponent<Storage>().bookList[0].value;
                        carriedBookPrefab = storage.GetComponent<Storage>().bookList[0].prefab;
                        storage.GetComponent<Storage>().bookList.RemoveAt(0);
                        storage.GetComponent<Storage>().booksInStorage--;
                        uiController.CloseStorageStatus();
                        storage.GetComponent<Storage>().BooksOnTop();
                        if(carriedBookPrefab != null)
                        {
                            currentState = WorkerState.StockShelves;
                        } else currentState = WorkerState.Idle;
                        workTimer = 0;

                        
                    }
                    
                }
                break;
                case WorkerState.StockShelves:
                    if (bookSpotFree == null)
                    {
                        gameController.GetAvailableBooks();
                        availableBookSpots = gameController.availableBookSpots;
                        randomIndex = Random.Range(0,availableBookSpots.Count);
                        bookSpotFree = availableBookSpots[randomIndex];
                        navMeshAgent.destination = bookSpotFree.transform.position;
                        bookSpotFree.isAvailable = false;

                    }
                    else if (bookSpotFree !=null)
                    {

                    
                        if (Vector3.Distance(this.transform.position, bookSpotFree.transform.position) < 2f)
                        {
                           FaceTarget(bookSpotFree.transform.position);

                            if(workTimer<=workTime)
                            {
                                workTimer+=Time.deltaTime;
                            }
                            else
                            {
                                GameObject newBook = Instantiate(carriedBookPrefab, bookSpotFree.transform.position, Quaternion.identity, bookSpotFree.transform);
                                newBook.transform.localRotation = Quaternion.Euler(0, 90, 0);
                                newBook.transform.localPosition = new Vector3(0,0.23f,0);
                                newBook.GetComponent<PickUpObject>().bookName = carriedBookName;
                                newBook.GetComponent<PickUpObject>().price = carriedBookPrice;
                                
                                carriedBookName = null;
                                carriedBookPrice = 0;
                                carriedBookPrefab = null;
                                bookSpotFree.isAvailable = false;
                                gameController.books++;
                                bookSpotFree = null;
                                //availableBookSpots.RemoveAt(randomIndex);
                                
                                //currentState = WorkerState.Idle;
                                workTimer = 0;
                                gameController.GetAvailableBooks();
                                availableBookSpots = gameController.availableBookSpots;
                                gameController.RestockShelves();
                                currentState = WorkerState.Idle;
                                gameController.restockInProgress = false;
                                
                            }
                        }
                    }
                    break;
            case WorkerState.Exit:
                    navMeshAgent.destination =(employeeDoor);
                if (Vector3.Distance(this.transform.position, employeeDoor) < 2f)
                {
                    gameController.restockInProgress = false;
                    Destroy(gameObject);
                }
               
                break;
                case WorkerState.Cleaning:
                
                       if(cleaning == false)
                       {
                            randomIndexCleaning = Random.Range(0,decorationToClean.Count);
                            cleanTarget = decorationToClean[randomIndexCleaning];
                            navMeshAgent.destination = cleanTarget.transform.position;
                            cleaning = true;
                       }
                        
                
                if (Vector3.Distance(this.transform.position, cleanTarget.transform.position) < 2f)
                {
                    FaceTarget(cleanTarget.transform.position);
                    if(workTimer<=workTime)
                    {
                        workTimer+=Time.deltaTime;
                    } else{
                        workTimer = 0f;
                        currentState = WorkerState.Idle;
                        cleaning = false;
                    }
                
                }
               
                break;
        }
    }

    // Change the worker's state to the specified state
    public void ChangeState(WorkerState newState)
    {
        currentState = newState;
    }
    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);  
    }
}

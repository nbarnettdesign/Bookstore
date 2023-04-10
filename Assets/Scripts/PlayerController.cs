using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
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
                }
                else if (hit.collider.CompareTag("CheckoutDesk"))
                {
                    WorkAtCheckoutDesk();
                }
                else if (hit.collider.CompareTag("Storage"))
                {
                    WorkAtStorage();
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

    public void WorkAtStorage()
    {
       
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);  
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CustomerState
{
    Idle,
    Walking,
    Deciding,
    LookingAtItem,
    PickingUpItem,
    Exiting,
    LineUp,
    WaitingInLine,
    Paying
}

public class Customer : MonoBehaviour
{
    public float walkSpeed = 1.5f;
    public float lookAtItemTime = 2f;
    public float pickupItemTime = 2f;

    public float pickupRange = 10f;
    public float pickupChance = .5f;
    public bool isInLine = false;
    public bool atPaymentPoint = false;
    private List<PickUpObject> pickupList;
    private int currentPickupIndex = 0;
    public NavMeshAgent navMeshAgent;
    public CustomerState currentState = CustomerState.Idle;
    private PickUpObject targetPickupObject;
    private float lookAtItemTimer = 0f;
    private float pickupItemTimer = 0f;
    private Vector3 exitPoint;
    private bool hasExited = false;
    public int pickedUpItemPrice;
    private bool hasItem;
    public int maxPickupChecks = 5;
    public int minPickupChecks = 1;
    private int pickupCheckCount;
    private int pickupCheckAmount;
    private PaymentLine frontDesk;
    public GameController gameController;
    public LinePoint availablelinePoint;

    void Start()
    {
        pickupList = new List<PickUpObject>();
        pickupList.AddRange(FindObjectsOfType<PickUpObject>());
        exitPoint = GameObject.FindGameObjectWithTag("ExitPoint").transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = pickupRange;
        pickupCheckCount = 0;
        pickupCheckAmount = Random.Range(minPickupChecks, maxPickupChecks);
        hasItem = false;
        frontDesk = FindObjectOfType<PaymentLine>();
        gameController = FindAnyObjectByType<GameController>();
        atPaymentPoint = false;

    }

    void Update()
    {
        switch (currentState)
        {
            case CustomerState.Idle:
                 if(pickupCheckCount <=pickupCheckAmount)
                 {
                    List<PickUpObject> availablePickupObjects = new List<PickUpObject>();
                    foreach (PickUpObject pickupObject in pickupList)
                    {
                        if (pickupObject.isAvailable)
                        {
                             availablePickupObjects.Add(pickupObject);
                        }
                    }
                
                    if (availablePickupObjects.Count > 0)
                    {
                         currentPickupIndex = Random.Range(0, availablePickupObjects.Count);
                        targetPickupObject = availablePickupObjects[currentPickupIndex];
                        //Debug.Log("SET STATE TO: WALKING");
                        currentState = CustomerState.Walking;
                        if(targetPickupObject != null)
                        {
                            navMeshAgent.SetDestination(targetPickupObject.transform.position);
                        }
                        
                        //Debug.Log("Walking to "+ targetPickupObject);
                        targetPickupObject.isAvailable = false;
                    }else currentState = CustomerState.Exiting;
                 } else currentState = CustomerState.Exiting;
                break;
            case CustomerState.Walking:
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    //Debug.Log("SET STATE TO: LOOKINGATITEM");
                    currentState = CustomerState.LookingAtItem;
                    lookAtItemTimer = 0f;
                }
                break;
                case CustomerState.Deciding:
                if (Random.value <= pickupChance)
                {
                    //Debug.Log("SET STATE TO: PICKINGUPITEM");
                    currentState = CustomerState.PickingUpItem;
                    lookAtItemTimer = 0f;
                    pickupItemTimer = 0f;
                    targetPickupObject.SetIsBeingCarried(true);
                    //Debug.Log("Im Going to Buy "+targetPickupObject);
                    hasItem=true;
                }
                else
                {
                    //Debug.Log("i Dont want "+targetPickupObject);
                    pickupCheckCount+=1;
                    targetPickupObject.isAvailable = true;
                    if (pickupCheckCount <=pickupCheckAmount)
                    {
                        //Debug.Log("SET STATE TO: IDLE");
                        currentState = CustomerState.Idle;
                        lookAtItemTimer = 0f;
                    }
                    else {
                    //Debug.Log("SET STATE TO: EXITING");
                    currentState = CustomerState.Exiting;
                    }
                }
                break;
            case CustomerState.LookingAtItem:
                if (lookAtItemTimer >= lookAtItemTime)
                {
                    if(!hasItem)
                    {
                    //Debug.Log("SET STATE TO: DECIDING");
                    currentState = CustomerState.Deciding;
                    }
                    else 
                    {
                        //Debug.Log("SET STATE TO: LINEUP");
                        currentState = CustomerState.LineUp;
                        targetPickupObject.isAvailable = true;
                    }
                }
                else
                {
                    if(targetPickupObject != null){
                        FaceTarget(targetPickupObject.transform.position);
                        lookAtItemTimer += Time.deltaTime;
                    } else currentState = CustomerState.Idle;
                }
                break;
            case CustomerState.PickingUpItem:
                if (pickupItemTimer >= pickupItemTime)
                {
                    if(targetPickupObject != null)
                    {
                        targetPickupObject.SetIsBeingCarried(false);
                        pickedUpItemPrice = targetPickupObject.price;
                        Destroy(targetPickupObject.gameObject);
                        gameController.books--;
                        //Debug.Log("SET STATE TO: LINEUP");
                        currentState = CustomerState.LineUp;
                        hasItem = true;
                    } else currentState = CustomerState.Idle;
                        
                    
                }
                else
                {
                    pickupItemTimer += Time.deltaTime;
                }
                break;
            case CustomerState.Exiting:
                 navMeshAgent.SetDestination(exitPoint);
                if (Vector3.Distance(transform.position, exitPoint)  <= 3f && !hasExited) 
                    {
                        //Debug.Log("GOODBYE FOREVER");
                        hasExited = true;
                        gameController.currentCustomerCount --;
                        gameController.RestockShelves();
                        Destroy(gameObject);
                    }
            break;
            case CustomerState.LineUp:
                
                availablelinePoint = frontDesk.IsRoomInLine();

                if(availablelinePoint !=null)
                {
                    availablelinePoint.isOccupied = true;
                    availablelinePoint.customer= this;
                    currentState = CustomerState.Paying;
                    navMeshAgent.SetDestination(availablelinePoint.transform.position);
                }
                else {
                    Debug.Log("ROOM IN LINE FAILED");
                    //Debug.Log("SET STATE TO: IDLE");
                    currentState = CustomerState.Idle;
                }
                break;
                case CustomerState.Paying:
                {
                    
                    if(availablelinePoint != null && availablelinePoint.inLine == false)
                    {
                        if(Vector3.Distance(availablelinePoint.transform.position, this.transform.position) <= 3f)
                        {
                            availablelinePoint.inLine = true;
                        }
                    }
                }
                break;
        }
    }

    public bool PickUpObject(PickUpObject pickUpObject, float pickupRange)
{
    if (currentState == CustomerState.PickingUpItem || currentState == CustomerState.Exiting)
        return false;

    if (pickupList.Contains(pickUpObject) && Vector3.Distance(transform.position, pickUpObject.transform.position) <= pickupRange)
    {
        currentState = CustomerState.PickingUpItem;
        targetPickupObject = pickUpObject;
        return true;
    }

    return false;
}
public void LeaveNow()
{
    navMeshAgent.SetDestination(exitPoint);
    gameController.currentGold+= pickedUpItemPrice;
    gameController.UIController.UpdateGoldText(gameController.currentGold);
    currentState = CustomerState.Exiting;
}

private void FaceTarget(Vector3 destination)
{
    Vector3 lookPos = destination - transform.position;
    lookPos.y = 0;
    Quaternion rotation = Quaternion.LookRotation(lookPos);
    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);  
}

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PaymentLine : MonoBehaviour
{
    public float paymentTime = 10f; // Time it takes to pay in seconds
    private float paymentTimer = 0f; // Maximum number of customers in line
    public GameController gameController;
    public List<LinePoint> linePoints = new List<LinePoint>(); // List of points in the line
    private List<Customer> customersInLine = new List<Customer>(); // List of customers in line
    public LinePoint paymentPoint;
    public Transform cashierPoint;
    public bool cashierAtTill;
    public bool cashierAssigned;
    public Worker currentCashier;
    public bool playerIsCashier;
    public GameObject playerTimerCanvas;
    public Image playerTimerImage;

    private void Start()
    {
        playerIsCashier = false;
        gameController = FindAnyObjectByType<GameController>();
        // Collect all the line points
        foreach (Transform child in transform)
        {
            // Check if the child has the LinePoint script attached
            LinePoint linePoint = child.GetComponent<LinePoint>();
            if (linePoint != null)
            {
                // Add the child's transform to the list
                linePoints.Add(linePoint);
            }
        }
        paymentTimer = 0f;
    }

    // Check if there is room at the end of the line for a customer to join
    public LinePoint IsRoomInLine()
    {
        foreach(LinePoint linePoint in linePoints)
        {
            if(linePoint.isOccupied==false)
            {
                return linePoint;
            }
        }
        return null;
    }

    // Add a customer to the end of the line and make them line up using their nav mesh
    public void AddToLine(Customer customer)
    {
        if (IsRoomInLine())
        {
            customersInLine.Add(customer);
            int lastPosition = customersInLine.Count - 1;
            customer.navMeshAgent.SetDestination(linePoints[lastPosition].transform.position);
            linePoints[lastPosition].isOccupied = true;
        }
    }

    // Move the remaining customers in the line one spot forward
    private void MoveLine()
    {
        for (int i = 1; i < linePoints.Count; i++)
        {
            //Debug.Log("i= "+i +" Name= "+linePoints[i]);
           // Debug.Log("Count: "+linePoints.Count);
              
              if (linePoints[i].isOccupied)
            {
                linePoints[i-1].customer = linePoints[i].customer;
                linePoints[i-1].customer.navMeshAgent.SetDestination(linePoints[i-1].transform.position);
                linePoints[i-1].customer.availablelinePoint = linePoints[i-1];
                
                if(i == linePoints.Count -1)
                {
                    linePoints[i].isOccupied = false;
                    linePoints[i].inLine = false;
                    linePoints[i].customer = null;
                }
            }
            else
            {
                linePoints[i-1].isOccupied = false;
                linePoints[i-1].inLine = false;
                linePoints[i-1].customer = null;
                break;
            }
            
            // If the next line point is not occupied, end the loop
        /*if (linePoints[i-1].isOccupied == false)
        {
            break;
        }*/
            
        }
    }


    private void Update()
    {
        // Check if the payment point is available and there are customers in line
        if (paymentPoint.isOccupied && paymentPoint.inLine&&cashierAtTill)
        {
           if(paymentTimer<=paymentTime)
           {
                paymentTimer += Time.deltaTime;
                if(currentCashier != null)
                {
                    currentCashier.GetComponent<Worker>().timerCanvas.SetActive(true);
                    currentCashier.GetComponent<Worker>().timerImage.fillAmount = paymentTimer/paymentTime;
                    playerTimerCanvas.SetActive(false);
                }
                else if(playerIsCashier)
                {
                    playerTimerCanvas.SetActive(true);
                    playerTimerImage.fillAmount = paymentTimer/paymentTime;
                }
           }
           else
           {
            //paymentPoint.inLine = false;
            paymentPoint.customer.LeaveNow();
            paymentTimer = 0f;
            paymentPoint.inLine = false;
            if(currentCashier != null)
                {
                    currentCashier.GetComponent<Worker>().timerCanvas.SetActive(false);
                }
                else if(playerIsCashier){
                    playerTimerCanvas.SetActive(false);
                }
            MoveLine();
           }
        }
        if (paymentPoint.isOccupied == false)
        {
            gameController.cashierNeeded = false;
        }
        if(paymentPoint.isOccupied == true)
        {
            gameController.cashierNeeded = true;
        }
    }
}

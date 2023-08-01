using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public GameController gameController;
    public UIController uiController;
    public GameObject workerPrefab;
    public Transform door;
    public int hireCost;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        uiController = FindObjectOfType<UIController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        if(gameController.currentGold >= hireCost)
        {
            gameController.currentGold -= hireCost;
            uiController.UpdateGoldText(gameController.currentGold);
            Instantiate(workerPrefab, door);
        }

    }
}

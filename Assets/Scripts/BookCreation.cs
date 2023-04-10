using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookCreation : MonoBehaviour
{
    public GameController gameController;
    public UIController uiController;
    public Storage storage;
    public Button nextButton;
    public Button previousButton;
    public List<GameObject> bookPages;
    public GameObject currentBookPage;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        uiController = FindObjectOfType<UIController>();
        storage = FindObjectOfType<Storage>();
        UpdateButtonInteractability(0);
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        uiController.CloseWindows();
        uiController.OpenCreationStatus();
    }
public void NextPage()
{
    int currentIndex = bookPages.IndexOf(currentBookPage);

    if (currentIndex < bookPages.Count - 1)
    {
        currentIndex++;
        currentBookPage.SetActive(false);
        currentBookPage = bookPages[currentIndex];
        currentBookPage.SetActive(true);
        UpdateButtonInteractability(currentIndex);
    }
}

public void PreviousPage()
{
    int currentIndex = bookPages.IndexOf(currentBookPage);

    if (currentIndex > 0)
    {
        currentIndex--;
        currentBookPage.SetActive(false);
        currentBookPage = bookPages[currentIndex];
        currentBookPage.SetActive(true);
        UpdateButtonInteractability(currentIndex);
    }
}

private void UpdateButtonInteractability(int currentIndex)
{
    if (currentIndex == 0)
    {
        // Disable previous button
        previousButton.interactable = false;
    }
    else
    {
        // Enable previous button
        previousButton.interactable = true;
    }

    if (currentIndex == bookPages.Count - 1)
    {
        // Disable next button
        nextButton.interactable = false;
    }
    else
    {
        // Enable next button
        nextButton.interactable = true;
    }
}

}

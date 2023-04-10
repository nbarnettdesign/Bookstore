using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookUI : MonoBehaviour
{
    public TextMeshProUGUI bookName;
    public TextMeshProUGUI bookValue; 
    public void SetNameAndValue(string name, int value)
{
    bookName.text = name;
    bookValue.text = value.ToString() + " Gold";
}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePoint : MonoBehaviour
{
    public bool isOccupied;
    public bool inLine;
    public Transform occupyingTransform;
    public Customer customer;

    private void Start() {
        occupyingTransform = this.transform;
    }
}

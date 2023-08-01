using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlacement : MonoBehaviour
{
  private List<Collider> collidingWith = new List<Collider>();
  [SerializeField] private BuildingManager buildingManager;
  [SerializeField] private Material noPlaceMaterial;
  private Material placeMaterial;
  [SerializeField] private MeshRenderer meshRenderer;
  public bool isPlaced;
  public int value;
    // Start is called before the first frame update
    void Start()
    {
        if(buildingManager == null)
        {
          buildingManager = FindObjectOfType<BuildingManager>();
        }
          
        placeMaterial = meshRenderer.material;
    }

   private void OnTriggerEnter(Collider other) 
   {
    //Debug.Log(other);
     if(!isPlaced)
     {
      if(other.name != "Floor" && other.name != "Camera Bounds")
      {
          buildingManager.canPlace = false;
          meshRenderer.material = noPlaceMaterial;
          collidingWith.Add(other);
      }
     }
     
   }

   private void OnTriggerExit(Collider other) 
   {
    if(!isPlaced)
    {
      if(collidingWith.Contains(other))
      {
        collidingWith.Remove(other);
        if(collidingWith.Count == 0)
        {
          buildingManager.canPlace = true;
          meshRenderer.material = placeMaterial;
        }  
      }
    }
    
   } 
}

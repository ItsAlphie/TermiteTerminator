using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> inventoryItems; 
    public List<GameObject> brokenInventoryItems;
    private static InventoryManager _instance;
    public static InventoryManager Instance{
        get{
            if(_instance == null){
                Debug.LogError("InventoryManager instance is null");
            }
            return  _instance;  
        }
    }

    private void Awake(){
        _instance = this;
    }

    public void addItem(GameObject gameObject){
        inventoryItems.Add(gameObject);
    }

    public void removeItem(GameObject gameObject){
        inventoryItems.Remove(gameObject);
    }

    public void addBrokenItem(GameObject gameObject){
        brokenInventoryItems.Add(gameObject);
    }

    public void removeBrokenItem(GameObject gameObject){
        brokenInventoryItems.Remove(gameObject);
    }

    public void moveToBroken(GameObject gameObject){
        removeItem(gameObject);
        addBrokenItem(gameObject);
    }

    public void removeFromBroken(GameObject gameObject){
        removeBrokenItem(gameObject);
        addItem(gameObject);
    }
}

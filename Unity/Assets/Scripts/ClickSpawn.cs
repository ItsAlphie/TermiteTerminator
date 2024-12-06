using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSpawn : MonoBehaviour
{   
    [SerializeField] GameObject TowerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1)){
            Debug.Log("Click");
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject tower = Instantiate(TowerPrefab, mousePosition, Quaternion.identity);
            tower.GetComponent<BasicTower>().State = BasicTower.TowerState.Bought;
            InventoryManager.Instance.addItem(tower);
            TowerSpawner.Instance.towers.Add(tower);
        }

    }
}

   

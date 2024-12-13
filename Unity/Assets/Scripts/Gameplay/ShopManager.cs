using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Uduino;
using UnityEngine;
using UnityEngine.Events;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopPrices shopPrices;    
    private ShopPrices shopPricesCopy;    

    void Awake(){
        shopPricesCopy = Instantiate(shopPrices);
    }
    
    public int getPrice(){
        return shopPricesCopy.priceList.Find(x => x.tag == gameObject.tag).buyPrice;
    }

    public int getTotalAmount(){
        return shopPricesCopy.priceList.Find(x => x.tag == gameObject.tag).totalAmount;
    }
    
    public bool buyItem(){
        var shopItem = shopPricesCopy.priceList.Find(x => x.tag == gameObject.tag);
        if((shopItem.totalAmount  > 0) && (MoneyManager.Instance.CurrentMoney > shopItem.buyPrice)&& (gameObject.GetComponent<BasicTower>().State == BasicTower.TowerState.Available)){
            MoneyManager.Instance.deductMoney(shopItem.buyPrice);
            shopItem.totalAmount--;
            InventoryManager.Instance.addItem(gameObject);
            gameObject.GetComponent<BasicTower>().State = BasicTower.TowerState.Bought;
            Debug.Log("Inventory:" + InventoryManager.Instance.inventoryItems);
            return true;
        }
        else if(gameObject.GetComponent<BasicTower>().State == BasicTower.TowerState.Available){
            UIManager.Instance.showInsufficientFundsPopUp(gameObject.transform.position);
            SoundController.instance.PlayErrorSound();
        }
        
        return false;
    }

    public bool sellItem(){
        if(gameObject.GetComponent<BasicTower>().State == BasicTower.TowerState.Bought) {
            var shopItem = shopPricesCopy.priceList.Find(x => x.tag == gameObject.tag);
            MoneyManager.Instance.addMoney(shopItem.sellPrice);
            InventoryManager.Instance.removeItem(gameObject);
            gameObject.GetComponent<BasicTower>().State = BasicTower.TowerState.Available;
            shopItem.totalAmount++;
            return true;
        }
        return false;
    }
 
    
}

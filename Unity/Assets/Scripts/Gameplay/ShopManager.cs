using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

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
        if((shopItem.totalAmount  > 0) && (MoneyManager.Instance.CurrentMoney > shopItem.buyPrice)&& gameObject.GetComponent<BasicTower>().State == BasicTower.TowerState.Available){
            MoneyManager.Instance.deductMoney(shopItem.buyPrice);
            shopItem.totalAmount--;
            InventoryManager.Instance.addItem(gameObject);
            gameObject.GetComponent<BasicTower>().State = BasicTower.TowerState.Bought;
            Debug.Log("Inventory:" + InventoryManager.Instance.inventoryItems);
            return true;
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

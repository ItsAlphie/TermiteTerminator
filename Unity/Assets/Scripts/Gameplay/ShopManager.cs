using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopPrices shopPrices;    
    private static ShopManager _instance;
    public static ShopManager Instance{
        get{
            if(_instance == null){
                Debug.LogError("ShopManager instance is null");
            }
            return  _instance;  
        }
    }

    private void Awake(){
        _instance = this;
    }

    
    public int getPrice(GameObject prefab){
        return shopPrices.priceList.Find(x => x.shopItemPrefab = prefab).buyPrice;
    }

    public int getTotalAmount(GameObject prefab){
        return shopPrices.priceList.Find(x => x.shopItemPrefab = prefab).totalAmount;
    }
    
    public bool buyTower(GameObject prefab){
        var shopItem = shopPrices.priceList.Find(x => x.shopItemPrefab = prefab);
        if((shopItem.totalAmount  > 0) && (MoneyManager.Instance.CurrentMoney > shopItem.buyPrice)){
            MoneyManager.Instance.deductMoney(shopItem.buyPrice);
            shopItem.totalAmount--;
            InventoryManager.Instance.addItem(prefab);
            return true;
        }
        return false;
    }

    public bool sellTower(GameObject prefab){
        if(InventoryManager.Instance.inventoryItems.Contains(prefab)){
            var shopItem = shopPrices.priceList.Find(x => x.shopItemPrefab = prefab);
            MoneyManager.Instance.addMoney(shopItem.sellPrice);
            InventoryManager.Instance.removeItem(prefab);
            shopItem.totalAmount++;
            return true;
        }
        return false;
    }
    
}

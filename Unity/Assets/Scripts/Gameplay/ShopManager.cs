using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        return shopPrices.priceList.Count(x => x.shopItemPrefab == prefab);   
    }
    
    public bool buyTower(GameObject prefab){
        var shopItem = shopPrices.priceList.Find(x => x.shopItemPrefab = prefab);
        if((shopItem != null ) && (MoneyManager.Instance.CurrentMoney > shopItem.buyPrice)){
            MoneyManager.Instance.deductMoney(shopItem.buyPrice);
            shopPrices.priceList.Remove(shopItem);
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
            shopPrices.priceList.Add(shopItem);
            return true;
        }
        return false;
    }
    
}

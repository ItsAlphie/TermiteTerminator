using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ShopPrices.asset", menuName ="ScriptableObjects/ShopPrices")]
public class ShopPrices : ScriptableObject
{
    [System.Serializable]
    public class ShopItem{
        public string shopItemName;
        public GameObject shopItemPrefab;
        public int buyPrice;
        public int sellPrice;
    }

    public List<ShopItem> priceList;
    
}

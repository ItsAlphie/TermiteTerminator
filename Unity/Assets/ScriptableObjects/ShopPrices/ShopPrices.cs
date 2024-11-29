using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ShopPrices.asset", menuName ="ScriptableObjects/ShopPrices")]
public class ShopPrices : ScriptableObject
{
    [System.Serializable]
    public class ShopItem{
        public string tag;
        public int buyPrice;
        public int sellPrice;
        public int totalAmount;
    }

    public List<ShopItem> priceList;
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ShopPrices.asset", menuName ="ScriptableObjects/ShopPrices")]
public class ShopPrices : ScriptableObject
{
    [System.Serializable]
    public class shopItem{
        public string shopItemName;
        public GameObject shopItemPrefab;
        public int price;
        public int totalAmount; //copied at runtime in ShopManager
    }

    public List<shopItem> shopPrices;
    
}

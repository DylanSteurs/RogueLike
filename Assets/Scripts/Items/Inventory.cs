using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Consumables> items = new List<Consumables>();
    public int maxItems = 2;
    public bool AddItem(Consumables item)
    {
        if (items.Count < maxItems)
        {
            items.Add(item);
            return true;
        }
        return false;
    }
    public void DropItem(Consumables item)
    {
        items.Remove(item);
    }
}

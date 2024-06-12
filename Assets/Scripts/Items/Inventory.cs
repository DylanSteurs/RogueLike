using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<Consumables> Items = new List<Consumables>();
    public int MaxItems = 8;

    public bool AddItem(Consumables item)
    {
        if (Items.Count < MaxItems)
        {
            Items.Add(item);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DropItem(Consumables item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
        }
    }
}
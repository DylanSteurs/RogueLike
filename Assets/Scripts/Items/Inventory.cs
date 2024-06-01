using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public List<Consumables> Items { get; private set; } = new List<Consumables>();
    public int maxItems ;

    public bool AddItem(Consumables item)
    {
        if (Items.Count < maxItems)
        {
            Items.Add(item);
            return true;
        }
        return false;
    }
    public void DropItem(Consumables item)
    {
        Items.Remove(item);
    }
    public bool IsFull
    {
        get { return Items.Count >= maxItems; }
    }
}

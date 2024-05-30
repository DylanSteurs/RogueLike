using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumables : MonoBehaviour
{
    private void Start()
    {
        GameManager.Get.AddItem(this);
    }
    public enum ItemType
    {
        HealthPotion,
        Fireball,
        ScrollOfConfusion
    }
    private ItemType type;

    public ItemType Type
    {
        get { return type; }
    }
}

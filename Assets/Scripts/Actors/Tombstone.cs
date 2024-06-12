using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombstone : MonoBehaviour
{
    void Start()
    {
        GameManager.Get.AddTombStone(this);
    }
}

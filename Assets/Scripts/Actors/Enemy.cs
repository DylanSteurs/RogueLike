using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Actor))]
public class Enemy : MonoBehaviour
{
    private AStar Algorithm;
    public Actor Target;
    public bool IsFighting = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Get.AddEnemy(GetComponent<Actor>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor), typeof(AStar))]
public class Enemy : MonoBehaviour
{
    public Actor Target { get; set; }
    public bool IsFighting { get; private set; } = false;
    private AStar algorithm;
    private int confused = 0;

    void Start()
    {
        algorithm = GetComponent<AStar>();

        GameManager.Get.AddEnemy(GetComponent<Actor>());
    }

    void Update()
    {
        RunAI();
    }

    public void MoveAlongPath(Vector2Int targetPosition)
    {
        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        Vector2 direction = algorithm.Compute((Vector2Int)gridPosition, (Vector2Int)targetPosition);
        Action.Move(GetComponent<Actor>(), direction);
    }

    public void RunAI()
    {
        if (Target == null || Target.Equals(null))
        {
            Target = GameManager.Get.Player;
        }

        if (Target == null)
        {
            return;
        }
        if (confused > 0)
        {
            confused--;
            UIManager.Instance.AddMessage($"{name} is confused and cannot act", Color.cyan);
            return;
        }

        Vector3Int targetGridPosition = MapManager.Get.FloorMap.WorldToCell(Target.transform.position);

        Vector3Int currentGridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        if (IsFighting || GetComponent<Actor>().FieldOfView.Contains(targetGridPosition))
        {
            if (!IsFighting)
            {
                IsFighting = true;
            }

            float distanceToTarget = Vector3.Distance(transform.position, Target.transform.position);

            if (distanceToTarget < 1.5f)
            {
                Action.Hit(GetComponent<Actor>(), Target);
            }
            else
            {
                MoveAlongPath((Vector2Int)targetGridPosition);
            }
        }
    }
    public void Confuse()
    {
        confused = 8;
    }
}
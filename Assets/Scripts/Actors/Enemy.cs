using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor), typeof(AStar))]
public class Enemy : MonoBehaviour
{
    // Variables
    public Actor Target { get; set; }
    public bool IsFighting { get; private set; } = false;
    private AStar algorithm;

    // Start is called before the first frame update
    void Start()
    {
        // Set algorithm to the AStar component of this script
        algorithm = GetComponent<AStar>();

        // Add the Actor component to the GameManager's Enemies list
        GameManager.Get.AddEnemy(GetComponent<Actor>());
    }

    // Update is called once per frame
    void Update()
    {
        RunAI();
    }

    // Function to move along the path to the target position
    public void MoveAlongPath(Vector2Int targetPosition)
    {
        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        Vector2 direction = algorithm.Compute((Vector2Int)gridPosition, (Vector2Int)targetPosition);
        Action.Move(GetComponent<Actor>(), direction);
    }

    // Function to run the enemy AI
    public void RunAI()
    {
        // Als target null is of vernietigd is, stel target in op de speler (vanuit GameManager)
        if (Target == null || Target.Equals(null))
        {
            Target = GameManager.Get.Player;
        }

        // Als de target nog steeds null is na de poging om deze toe te wijzen, keer dan terug
        if (Target == null)
        {
            return;
        }

        // Converteer de positie van de target naar een gridpositie
        Vector3Int targetGridPosition = MapManager.Get.FloorMap.WorldToCell(Target.transform.position);

        // Controleer eerst of er al gevochten wordt, omdat het controleren van het gezichtsveld meer CPU kost
        Vector3Int currentGridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        if (IsFighting || GetComponent<Actor>().FieldOfView.Contains(targetGridPosition))
        {
            // Als de vijand niet aan het vechten was, zou hij nu moeten vechten
            if (!IsFighting)
            {
                IsFighting = true;
            }

            // Bereken de afstand tot de target
            float distanceToTarget = Vector3.Distance(transform.position, Target.transform.position);

            // Als de afstand minder is dan 1.5, val dan de target aan
            if (distanceToTarget < 1.5f)
            {
                Action.Hit(GetComponent<Actor>(), Target);
            }
            else
            {
                // Anders, beweeg langs het pad naar de target
                MoveAlongPath((Vector2Int)targetGridPosition);
            }
        }
    }
}
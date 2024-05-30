using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public List<Actor> Enemies = new List<Actor>();
    public Actor Player { get; set; }
    public List<Consumables> Items { get; private set; } = new List<Consumables>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void AddEnemy(Actor enemy)
    {
        Enemies.Add(enemy);
    }

    public static GameManager Get { get => instance; }

    public Actor GetActorAtLocation(Vector3 location)
    {
        if (Player != null && Player.transform.position == location)
        {
            return Player;
        }
        foreach (Actor enemy in Enemies)
        {
            if (enemy != null && enemy.transform.position == location)
            {
                return enemy;
            }
        }
        return null;
    }
    public Consumables GetItemAtLocation(Vector3 location)
    {
        foreach (var item in Items)
        {
            if (item != null && item.transform.position == location)
            {
                return item;
            }
        }
        return null;
    }
    public GameObject CreateActor(string name, Vector2 position)
    {
        GameObject actor = Instantiate(Resources.Load<GameObject>($"Prefabs/{name}"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
        actor.name = name;
        return actor;
    }

    public void StartEnemyTurn()
    {
        foreach (Actor enemy in GameManager.Get.Enemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.RunAI();
            }
        }
    }
    public void RemoveEnemy(Actor enemy)
    {
        if (Enemies.Contains(enemy))
        {
            Enemies.Remove(enemy);
            Destroy(enemy.gameObject);
            Debug.Log($"{enemy.name} has been removed.");
        }
        else
        {
            Debug.Log("Enemy not found in the list.");
        }
    }
    public GameObject CreateItem(string name, Vector2 position)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>($"Prefabs/{name}"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
        AddItem(item.GetComponent<Consumables>());
        item.name = name;
        return item;
    }
    public void AddItem(Consumables item)
    {
        Items.Add(item);
    }
}

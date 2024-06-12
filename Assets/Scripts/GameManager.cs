using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public List<Actor> Enemies = new List<Actor>();
    public List<Ladder> Ladders { get; private set; } = new List<Ladder>();
    private List<Tombstone> Tombstones = new List<Tombstone>();

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
    public void AddLadder(Ladder ladder)
    {
        Ladders.Add(ladder);
    }
    public void AddTombStone(Tombstone stone)
    {
        Tombstones.Add(stone);
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
    public Ladder GetLadderAtLocation(Vector3 location)
    {
        foreach (Ladder ladder in Ladders)
        {
            if (ladder.transform.position == location)
            {
                return ladder;
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
    public void RemoveItem(Consumables item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
            Destroy(item.gameObject);
        }
    }
    public List<Actor> GetNearbyEnemies(Vector3 location)
    {
        List<Actor> nearbyEnemies = new List<Actor>();

        foreach (Actor enemy in Enemies)
        {
            if (Vector3.Distance(enemy.transform.position, location) < 5)
            {
                nearbyEnemies.Add(enemy);
            }
        }

        return nearbyEnemies;
    }
    public void ClearFloor()
    {
        foreach (var enemy in Enemies)
        {
            Destroy(enemy.gameObject);
        }
        Enemies.Clear();

        foreach (var item in Items)
        {
            Destroy(item.gameObject);
        }
        Items.Clear();

        foreach (var ladder in Ladders)
        {
            Destroy(ladder.gameObject);
        }
        Ladders.Clear();

        foreach (var stone in Tombstones)
        {
            Destroy(stone.gameObject);
        }
        Tombstones.Clear();
    }
        public void SavePlayerData()
    {
        if (Player != null)
        {
            var player = Player.GetComponent<Player>();
            PlayerPrefs.SetInt("MaxHitPoints", player.MaxHitPoints);
            PlayerPrefs.SetInt("HitPoints", player.HitPoints);
            PlayerPrefs.SetInt("Defense", player.Defense);
            PlayerPrefs.SetInt("Power", player.Power);
            PlayerPrefs.SetInt("Level", player.Level);
            PlayerPrefs.SetInt("XP", player.XP);
            PlayerPrefs.SetInt("XpToNextLevel", player.XpToNextLevel);
            PlayerPrefs.Save();
        }
    }

    public void LoadPlayerData()
    {
        if (Player != null)
        {
            var player = Player.GetComponent<Player>();
            player.MaxHitPoints = PlayerPrefs.GetInt("MaxHitPoints", 100); 
            player.HitPoints = PlayerPrefs.GetInt("HitPoints", 100);
            player.Defense = PlayerPrefs.GetInt("Defense", 10);
            player.Power = PlayerPrefs.GetInt("Power", 10);
            player.Level = PlayerPrefs.GetInt("Level", 1);
            player.XP = PlayerPrefs.GetInt("XP", 0);
            player.XpToNextLevel = PlayerPrefs.GetInt("XpToNextLevel", 100);
        }
    }

    public void DeletePlayerSave()
    {
        PlayerPrefs.DeleteKey("MaxHitPoints");
        PlayerPrefs.DeleteKey("HitPoints");
        PlayerPrefs.DeleteKey("Defense");
        PlayerPrefs.DeleteKey("Power");
        PlayerPrefs.DeleteKey("Level");
        PlayerPrefs.DeleteKey("XP");
        PlayerPrefs.DeleteKey("XpToNextLevel");
    }

    public void PlayerDied()
    {
        DeletePlayerSave();
    }

}

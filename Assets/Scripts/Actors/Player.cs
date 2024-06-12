using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    public Inventory inventory;
    private bool inventoryIsOpen = false;
    private bool droppingItem = false;
    private bool usingItem = false;
    private void Awake()
    {
        controls = new Controls();
    }

    private void Start()
    {
        GameManager.Get.Player = GetComponent<Actor>();
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }

    private void OnEnable()
    {
        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.SetCallbacks(null);
        controls.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryIsOpen)
            {
                Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
                if (direction.y > 0)
                {
                    UIManager.Get.Inventory.SelectPreviousItem();
                }
                else if (direction.y < 0)
                {
                    UIManager.Get.Inventory.SelectNextItem();
                }
            }
            else
            {
                Move();
            }
        }
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if (inventoryIsOpen)
        {
            UIManager.Get.InventoryUI.Hide();
            inventoryIsOpen = false;
            droppingItem = false;
            usingItem = false;
        }
    }

    private void Move()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        Debug.Log("roundedDirection");
        Action.MoveOrHit(GetComponent<Actor>(), roundedDirection);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.performed)
            {
                var item = GameManager.Get.GetItemAtLocation(transform.position);
                if (item != null)
                {
                    if (Inventory.AddItem(item))
                    {
                        item.gameObject.SetActive(false);
                        GameManager.Get.RemoveItem(item);
                        UIManager.Get.AddMessage($"You've picked up a {item.name}.", Color.yellow);
                    }
                    else
                    {
                        UIManager.Get.AddMessage("Your inventory is full.", Color.red);
                    }

                }
                else
                {
                    UIManager.Get.AddMessage("You could not find anything.", Color.yellow);
                }
            }
        }
    }
        public void OnDrop(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!inventoryIsOpen)
                {
                    InventoryUI.Show(GameManager.Get.Player.GetComponent<Inventory>().Items);
                    inventoryIsOpen = true;
                    droppingItem = true;
                }
            }
        }

        public void OnSelect(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (inventoryIsOpen)
                {
                    Consumables selectedItem = inventory.Items[InventoryUI.Selected];
                    inventory.DropItem(selectedItem);

                    if (droppingItem)
                    {
                        selectedItem.transform.position = transform.position;
                        GameManager.Get.AddItem(selectedItem);
                        selectedItem.gameObject.SetActive(true);
                    }
                    else if (usingItem)
                    {
                        UseItem(selectedItem);
                        Destroy(selectedItem.gameObject);
                    }

                    InventoryUI.Hide();
                    inventoryIsOpen = false;
                    droppingItem = false;
                    usingItem = false;
                }
            }
        }
        public void UseItem(Consumables item)
        {
            switch (item.Type)
            {
                case Consumables.ItemType.HealthPotion:
                    GetComponent<Actor>().Heal(5);
                    break;
                case Consumables.ItemType.Fireball:
                    {
                        var enemies = GameManager.Get.GetNearbyEnemies(transform.position);
                        foreach (var enemy in enemies)
                        {
                            enemy.DoDamage(8);
                            UIManager.Get.AddMessage($"Your fireball damaged the {enemy.name} for 8HP", Color.magenta);
                        }
                        break;
                    }

                case Consumables.ItemType.ScrollOfConfusion:
                    {
                        var enemies = GameManager.Get.GetNearbyEnemies(transform.position);
                        foreach (var enemy in enemies)
                        {
                            enemy.GetComponent<Enemy>().Confuse();
                            UIManager.Get.AddMessage($"Your scroll confused the {enemy.name}.", Color.magenta);
                        }
                        break;
                    }

            }
        }

        public void OnUse(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!inventoryIsOpen)
                {
                    InventoryUI.Show(GameManager.Get.Player.GetComponent<Inventory>().Items);
                    inventoryIsOpen = true;
                    usingItem = true;
                }
            }
        }
    private void CheckLadder()
    {
        // Vraag de GameManager naar een ladder op de huidige locatie
        Ladder ladder = GameManager.Get.GetLadderAtLocation(transform.position);

        if (ladder != null)
        {
            // Kijk of de ladder naar boven of naar beneden gaat
            if (ladder.GoesUp)
            {
                // Voer de functie MoveUp van MapManager uit
                MapManager.Get.MoveUp();
            }
            else
            {
                // Voer de functie MoveDown van MapManager uit
                MapManager.Get.MoveDown();
            }
        }
    }
} 

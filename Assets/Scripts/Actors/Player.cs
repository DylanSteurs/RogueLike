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
                    InventoryUI.SelectPreviousItem();
                }
                else if (direction.y < 0)
                {
                    InventoryUI.SelectNextItem();
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
            Vector3 playerPosition = transform.position;
            Consumables item = GameManager.Get.GetItemAtLocation(playerPosition);

            if (item == null)
            {
                Debug.Log("No items present.");
            }
            else if (inventory.IsFull)
            {
                Debug.Log("Inventory is full.");
            }
            else
            {
                inventory.AddItem(item);
                item.gameObject.SetActive(false);
                GameManager.Get.RemoveItem(item);
                Debug.Log($"Picked up {item.name}");
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
    private void UseItem(Consumables item)
    {
        // Implement the functionality for using an item
        // For now, we will just log the item's name
        Debug.Log($"Using item: {item.name}");
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
}

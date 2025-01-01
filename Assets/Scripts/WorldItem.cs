using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public Item itemData; // Reference to the ScriptableObject

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var inventory = collision.GetComponent<InventoryManager>();
            if (inventory != null)
            {
                if (inventory.AddItem(new InventoryItem
                {
                    itemName = itemData.itemName,
                    quantity = itemData.quantity,
                    icon = itemData.icon
                }))
                {
                    Debug.Log($"{itemData.itemName} added to inventory!");
                    Destroy(gameObject); // Remove the item from the world
                }
            }
        }
    }
}

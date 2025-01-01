using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public int maxSlots = 10;

    // Add item to the inventory
    public bool AddItem(InventoryItem newItem)
    {
        foreach (var item in inventory)
        {
            if (item.itemName == newItem.itemName)
            {
                item.quantity += newItem.quantity; // Stack existing items
                return true;
            }
        }

        if (inventory.Count < maxSlots)
        {
            inventory.Add(newItem);
            return true;
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    // Remove item from inventory
    public bool RemoveItem(string itemName, int quantity)
    {
        foreach (var item in inventory)
        {
            if (item.itemName == itemName)
            {
                item.quantity -= quantity;
                if (item.quantity <= 0) inventory.Remove(item);
                return true;
            }
        }
        Debug.Log("Item not found!");
        return false;
    }

    // Check if item exists
    public bool HasItem(string itemName)
    {
        foreach (var item in inventory)
        {
            if (item.itemName == itemName)
                return true;
        }
        return false;
    }
}

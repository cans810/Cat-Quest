using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Health Bar Settings")]
    public Slider healthBar;

    [Header("Time Settings")]
    public float decreaseRate = 1f; // Rate at which health decreases per second

    [Header("Sprite Renderer Settings")]
    public SpriteRenderer spriteRenderer; // Reference to the sprite renderer for color change
    public Color fullHealthColor = Color.green; // Color when health is full
    public Color lowHealthColor = Color.red; // Color when health is low

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Initialize the health bar
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        // Initialize sprite renderer
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = fullHealthColor; // Set the initial sprite color to full health
        }
    }

    void Update()
    {
        // Decrease health over time
        DecreaseHealth();

        // Update health bar value
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Update sprite color based on health
        if (spriteRenderer != null)
        {
            UpdateSpriteColor();
        }

        // Handle death (when health reaches zero)
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void DecreaseHealth()
    {
        currentHealth -= decreaseRate * Time.deltaTime; // Decrease health based on time

        // Make sure health doesn't go below 0
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    void UpdateSpriteColor()
    {
        // Lerp the color based on health
        float healthPercentage = currentHealth / maxHealth;
        spriteRenderer.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercentage);
    }

    void Die()
    {
        // Handle death here (e.g., playing animation, disabling character, etc.)
        Debug.Log("Character died!");
    }
}

using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Health Bar Settings")]
    public Slider healthBar;

    [Header("Time Settings")]
    public float decreaseRate = 1f; 

    [Header("Sprite Renderer Settings")]
    public SpriteRenderer spriteRenderer; 
    public Color fullHealthColor = Color.green; 
    public Color lowHealthColor = Color.red; 

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = fullHealthColor; 
        }
    }

    void Update()
    {
        DecreaseHealth();

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (spriteRenderer != null)
        {
            UpdateSpriteColor();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void DecreaseHealth()
    {
        currentHealth -= decreaseRate * Time.deltaTime; 

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    void UpdateSpriteColor()
    {
        float healthPercentage = currentHealth / maxHealth;
        spriteRenderer.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercentage);
    }

    void Die()
    {
        // Handle death here (e.g., playing animation, disabling character, etc.)
        Debug.Log("Character died!");
    }
}

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
    public Image healthImage; 

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (healthImage == null)
        {
            healthImage = GetComponent<Image>(); 
        }
    }

    void Update()
    {
        DecreaseHealth();

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (healthImage != null)
        {
            float fillAmount = currentHealth / maxHealth;
            healthImage.fillAmount = fillAmount;  
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

    void Die()
    {
        Debug.Log("Character died!");
    }
}

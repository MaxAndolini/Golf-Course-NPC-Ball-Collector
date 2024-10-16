using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private float health = 100f; // Player's health
    [SerializeField] private Image healthBar; // Reference to the health bar UI
    [SerializeField] private float healthDecreaseRate = 0.5f; // Rate at which health decreases

    // Reset function to restore health and update health bar
    public void Reset()
    {
        health = 100f; // Reset health to 100
        healthBar.fillAmount = 1f; // Update the health bar to full (normalized value)
    }

    private void Update()
    {
        if (!GameManager.Instance.NPCController.isStopped)
        {
            // Decrease health over time
            health = Mathf.Clamp(health - healthDecreaseRate * Time.deltaTime, 0, 100);

            // Update health bar (normalized value between 0 and 1)
            healthBar.fillAmount = health / 100f;

            // If health reaches zero, stop NPC actions
            if (health <= 0)
            {
                Debug.Log("NPC is exhausted!");

                GameManager.Instance.NPCController.GameOver();
            }
        }
    }

    // Method to get the current health
    public float GetHealth()
    {
        return health;
    }
}
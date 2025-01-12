using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;

    public Image HealthBar;
    private bool invincible;

    private void Start()
    {
        invincible = false;
        currentHealth = maxHealth;
    }

    public void SetInvincibility(bool state)
    {
        invincible = state;
    }

    public void TakeDamage(int damageAmount)
    {
        if (!invincible)
        {
            // Reduce the player's health by the damage amount.
            currentHealth -= damageAmount;
            UpdateHealthBar();
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Die");
        }
    }
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0.0f, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float fillAmount = (float)currentHealth / maxHealth;
        if (fillAmount > 1)
        {
            fillAmount = 1.0f;
        }

        HealthBar.fillAmount = fillAmount;
    }
}

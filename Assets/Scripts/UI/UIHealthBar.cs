using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private TextMeshProUGUI healthBarTextBox;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance != null)
        {
            Health playerHealth = GameManager.instance.player.pawn.GetComponent<Health>();
            if (playerHealth != null)
            {
                healthBarImage.fillAmount = playerHealth.HealthPercent();
                healthBarTextBox.text = playerHealth.currentHealth + " / " + playerHealth.maxHealth;

            }
        }
    }
}
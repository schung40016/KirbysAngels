using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceShop : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerMovement playerMovement;

    private int healthTier = 1;
    private int damageTier = 1;
    private int comboTier = 1;
    private int manaTier = 1;

    [SerializeField] private int healthUpgradeCost = 300;
    [SerializeField] private int damageUpgradeCost = 500;
    [SerializeField] private int ComboUpgradeCost = 400;
    [SerializeField] private int manaUpgradeCost = 300;

    [SerializeField] private Text hpTier;
    [SerializeField] private Text mnTier;
    [SerializeField] private Text cbTier;
    [SerializeField] private Text dmgTier;

    public void Start()
    {
        hpTier.text = healthTier.ToString();
        mnTier.text = manaTier.ToString();
        cbTier.text = comboTier.ToString();
        dmgTier.text = damageTier.ToString();
    }

    // Opens up the experience shop.
    public void OpenExperienceShop()
    {

    }

    // Allows player to buy and upgrade their max health.
    public void BuyHealthUpgrade()
    {
        if ( player.playerExp >= healthTier * 300 && healthTier < 5)
        {
            healthTier += 1;
            player.maxHealth += 100;
            player.currentHealth = player.maxHealth;
        }
    }

    // Allows player to buy and upgrade their max health.
    public void BuyManaUpgrade()
    {
        if (player.playerExp >= manaTier * 300 && manaTier < 5)
        {
            manaTier += 1;
            player.maxMana += 100;
            player.currentMana = player.maxMana;
        }
    }

    public int getHealthTier()
    {
        return healthTier;
    }

    public int getManaTier()
    {
        return manaTier;
    }
}

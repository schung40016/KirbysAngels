using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceShop : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private GameObject experiencePanel;
    [SerializeField] private GameObject errorMessage;
    [SerializeField] private GameObject boss;

    private int healthTier = 1;
    private int damageTier = 1;
    private int comboTier = 1;
    private int manaTier = 1;
    private int upgradeCounter = 0;

    [SerializeField] private int healthUpgradeCost = 300;
    [SerializeField] private int damageUpgradeCost = 500;
    [SerializeField] private int ComboUpgradeCost = 400;
    [SerializeField] private int manaUpgradeCost = 300;

    [SerializeField] private Text hpTier;
    [SerializeField] private Text mnTier;
    [SerializeField] private Text cbTier;
    [SerializeField] private Text dmgTier;

    [SerializeField] private Button hpButton;
    [SerializeField] private Button mnButton;
    [SerializeField] private Button cbButton;
    [SerializeField] private Button dmgButton;

    private void Start()
    {
        hpTier.text = healthTier.ToString();
        mnTier.text = manaTier.ToString();
        cbTier.text = comboTier.ToString();
        dmgTier.text = damageTier.ToString();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && player.GetState() && !pauseMenu.GetPauseState())
        {
            if (!experiencePanel.activeSelf)
            {
                experiencePanel.SetActive(true);
            }
            else
            {
                experiencePanel.SetActive(false);
            }
        }

        if (healthTier == 5)
        {
            hpButton.interactable = false;
        }
        if (manaTier == 5)
        {
            mnButton.interactable = false;
        }
        if (comboTier == 5)
        {
            cbButton.interactable = false;
        }
        if (damageTier == 5)
        {
            dmgButton.interactable = false;
        }
    }

    // Allows player to buy and upgrade their max health.
    public void BuyHealthUpgrade()
    {
        if (player.playerExp >= healthTier * 300 && healthTier < 5)
        {
            player.maxHealth += 100;
            player.currentHealth = player.maxHealth;
            player.healthBar.SetMaxHealth(player.maxHealth);
            SubtractExp(healthTier, healthUpgradeCost);
            healthTier += 1;
            InitBoss();
            errorMessage.SetActive(false);
        }
        else
        {
            GetException();
        }
    }

    public void BuyDamageUpgrade()
    {
        if (damageTier < 5)
        {
            damageTier += 1;
            InitBoss();
            errorMessage.SetActive(false);
        }
        else
        {
            GetException();
        }
    }

    // Allows player to buy and upgrade their max health.
    public void BuyManaUpgrade()
    {
        if (player.playerExp >= manaTier * 300 && manaTier < 5)
        {
            player.maxMana += 100;
            player.currentMana = player.maxMana;
            SubtractExp(manaTier, manaUpgradeCost);
            manaTier += 1;
            InitBoss();
            errorMessage.SetActive(false);
        }
        else
        {
            GetException();
        }
    }

    private void InitBoss()
    {
        upgradeCounter++;
        if (upgradeCounter == 6)
        {
            boss.SetActive(true);
        }
    }

    public void GetException()
    {
        errorMessage.SetActive(true);
    }

    public void SubtractExp(int tier, int cost) 
    {
        player.playerExp -= tier * cost;
    }

    public int GetHealthTier()
    {
        return healthTier;
    }

    public int GetManaTier()
    {
        return manaTier;
    }

    public int GetDamageTier()
    {
        return damageTier;
    }

    public int GetUpgradeCounter()
    {
        return upgradeCounter;
    }
}

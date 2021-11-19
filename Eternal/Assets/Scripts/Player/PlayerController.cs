using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //public GameObject hp_pack;
    //public GameObject exp_pack;

    public HealthBar healthBar;
    public ManaBar manaBar;
    public Text experience;

    public int maxHealth = 100;
    public int currentHealth;

    private bool isAlive = true;
    [SerializeField] private GameObject deathScreen;

    public int maxMana = 100;
    public int currentMana;
    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);

    public int playerExp = 0;

    [SerializeField] private float timeStamp = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; 
        healthBar.SetMaxHealth(maxHealth);
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
        experience.text = "Exp: " + playerExp;
    }

    private void Update()
    {
        experience.text = "Exp: " + playerExp;
    }

    private IEnumerator RegenMana()
    {
        // Delays out mana regeneration.
        yield return new WaitForSeconds(timeStamp);

        while ( currentMana < maxMana)
        {
            currentMana += maxMana / 100;
            manaBar.SetMana(currentMana);
            yield return regenTick;
        }
    }

    public void UseMana(int mana)
    {
        currentMana -= mana;
        manaBar.SetMana(currentMana);
        StartCoroutine(RegenMana());
    }

    public void TakeDamage(int damage)
    {
        currentHealth-= damage;

        if (currentHealth <= 0)
        {
            //Health point is below or equal to zero, so player dead.
            Debug.Log("Player Dead. Game Over.");
            Invoke(nameof(KillPlayer), 0.5f);
        }

        healthBar.SetHealth(currentHealth);
    }

    private void KillPlayer()
    {
        isAlive = false;
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public bool GetState()
    {
        return isAlive;
    }
}

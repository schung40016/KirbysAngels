using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject hp_pack;
    public GameObject exp_pack;

    public HealthBar healthBar;
    public ManaBar manaBar;

    public int maxHealth = 100;
    public int currentHealth;

    public int maxMana = 100;
    public int currentMana;
    private bool isRegenMana = false;
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

    private void OnTriggerEnter(Collider other)
    {

        //Flaw here, it may destory all the hp pac or exp pack at the end.
        //But it shouldn't be an issue for prfabs
        //To Do: Need to find a way to see what object was touched.

        if (other.name == "Health Point")
        {
            float distance_hp = Vector3.Distance(other.transform.position, this.transform.position);
            if (distance_hp <= 2.5f)
            {
                Debug.Log("HP point got and increase 100 health point");
                currentHealth += 50;
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }

                //Destroy(hp_pack);
            }
        }
        else if (other.name == "Exp Point")
        {
            float distance_exp = Vector3.Distance(other.transform.position, this.transform.position);

            if (distance_exp <= 2.5f)
            {
                Debug.Log("EXP point got and increase 100 EXP point");
                playerExp += 100;

                //Destroy(exp_pack);
            }
        }
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
        Destroy(gameObject);
    }
}

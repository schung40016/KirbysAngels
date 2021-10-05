using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int playerHealth = 100;
    [SerializeField] private int experiencePoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getHealth()
    {
        return playerHealth;
    }

    public int getExperience()
    {
        return experiencePoints;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "New Move")]
public class Move : ScriptableObject
{
    [SerializeField] private List<KeyCode> movesKeyCodes;       // List of moves.
    [SerializeField] private Moves moveType;             // Defines the moves.
    [SerializeField] private int comboPriority = 0;             // Gives priority to harder moves.
    [SerializeField] private int damage;
    [SerializeField] private float movetime = 0;
    [SerializeField] private float knockBackMultiplier = 1;
    [SerializeField] private Vector3 knockBackDirection = new Vector3(1,1,1);
 
    public bool isMoveAvailable(List<KeyCode> playerKeyCodes)
    {
        int comboIndex = 0;

        for (int i = 0; i < playerKeyCodes.Count; i++)
        {
            if (playerKeyCodes[i] == movesKeyCodes[comboIndex])
            {
                comboIndex++;
                if (comboIndex == movesKeyCodes.Count)
                {
                    return true;
                }
            }
            else
            {
                comboIndex = 0;
            }
        }
        return false;
    }

    // Getters for class properties.
    public int GetMoveComboCount()
    {
        return movesKeyCodes.Count;
    }

    public int GetMoveComboPriority()
    {
        return comboPriority;
    }

    public Moves GetMove()
    {
        return moveType;
    }

    public int GetDamage()
    {
        return damage;
    }

    public float GetMoveTime()
    {
        return movetime;
    }

    public float GetKnockBackMultiplier()
    {
        return knockBackMultiplier;
    }

    public Vector3 GetKnockBackDirection()
    {
        return knockBackDirection;
    }
}

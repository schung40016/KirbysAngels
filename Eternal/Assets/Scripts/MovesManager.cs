using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesManager : MonoBehaviour
{
    [SerializeField] List<Move> availablesMoves; 
    PlayerMovement playerController;
    ControlManager controlManager;

    // Start is called before the first frame update
    void Awake()
    {
        playerController = FindObjectOfType<PlayerMovement>();
        controlManager = FindObjectOfType<ControlManager>();
        availablesMoves.Sort(Compare);
    }

    public bool CanMove(List<KeyCode> keycodes)
    {
        foreach (Move move in availablesMoves)
        {
            if (move.isMoveAvailable(keycodes))
            {
                return true;
            }
        }
        return false;
    }

    public void PlayMove(List<KeyCode> keycodes)
    {
        foreach (Move move in availablesMoves)
        {
            if (move.isMoveAvailable(keycodes))
            {
                playerController.PlayMove(move.GetMove(), move.GetMoveComboPriority());
                break;
            }
        }
    }

    // Compares which moves has higher priority.
    public int Compare(Move move1, Move move2)
    {
        return Comparer<int>.Default.Compare(move2.GetMoveComboPriority(), move1.GetMoveComboPriority());
    }
}

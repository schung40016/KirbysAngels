// Thanks to Fadrik for providing the tutorial for the combo system.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour
{
    [SerializeField] private float ComboResetTime = 0.5f;   // Provide lee way time for performing combos.
    [SerializeField] List<KeyCode> KeysPressed;             // Holds all the moves for combo anaylsis.
    [SerializeField] Text controlsTestText;
    MovesManager movesManager;

    // Start is called before the first frame update
    void Awake()
    {
        if (movesManager == null)
        {
            movesManager = FindObjectOfType<MovesManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectPressedKey();
        PrintControls();
    }

    // Pinpoints which key from the keyboard is pressed.
    public void DetectPressedKey()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                KeysPressed.Add(kcode);
                if (!movesManager.CanMove(KeysPressed))
                {
                    StopAllCoroutines();
                }

                StartCoroutine(ResetComboTimer());
            }
        }
    }

    // Resets combo when a move is performed.
    public void ResetCombo()
    {
        KeysPressed.Clear();
    }

    // Resets combo timer.
    IEnumerator ResetComboTimer()
    {
        yield return new WaitForSeconds(ComboResetTime);
        movesManager.PlayMove(KeysPressed);
        KeysPressed.Clear();
    }

    // Prints the controls pressed.
    public void PrintControls()
    {
        controlsTestText.text = "Keys Pressed (";

        Debug.Log(KeysPressed.Count);

        foreach (KeyCode kcode in KeysPressed)
        {
            controlsTestText.text += kcode + ",";
        }

        controlsTestText.text += ")";
    }
}

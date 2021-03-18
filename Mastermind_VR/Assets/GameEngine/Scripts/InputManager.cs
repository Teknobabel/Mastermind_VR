using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public static IEnumerator GetInput () {

        bool waitingForInput = true;

        while (waitingForInput) {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameManager.Instance.m_playerChoice = 1; 
                waitingForInput = false;
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                 GameManager.Instance.m_playerChoice = 2; 
                 waitingForInput = false;
            }

            yield return null;
        }
        
        Debug.Log("Player chose: " + GameManager.Instance.m_playerChoice);
    }
}

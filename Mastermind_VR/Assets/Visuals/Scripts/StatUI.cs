using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    public GameObject m_movementIndicator;
    public GameObject m_movementIndicatorLarge;
    public Image m_fill;
    public Text m_nameText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeMovementIndicatorState (bool enabled)
    {
        m_movementIndicator.SetActive(enabled);
        m_movementIndicatorLarge.SetActive(false);
        // if (enabled) {
        //     m_movementIndicator.SetActive(enabled);
        // } else {

        // }
    }

    public void ChangeLargeMovementIndicatorState (bool enabled)
    {
        m_movementIndicator.SetActive(false);
        m_movementIndicatorLarge.SetActive(enabled);
    }
}

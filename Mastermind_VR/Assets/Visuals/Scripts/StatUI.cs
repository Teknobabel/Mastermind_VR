using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    public GameObject m_movementIndicator;
    public Image m_fill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeMovementIndicatorState (bool enabled)
    {
        m_movementIndicator.SetActive(enabled);
        // if (enabled) {
        //     m_movementIndicator.SetActive(enabled);
        // } else {

        // }
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}

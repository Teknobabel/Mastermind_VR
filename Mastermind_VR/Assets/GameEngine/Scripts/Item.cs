using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    public enum Duration
    {
        Forever,
        NumTurns,
        VariableState,
    }
    public string m_name;
    public string m_portraitName;
    public Duration m_duration = Duration.Forever;
}

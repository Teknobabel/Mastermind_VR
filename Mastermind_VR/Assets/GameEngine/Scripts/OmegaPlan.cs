using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OmegaPlan", menuName = "ScriptableObjects/Omega Plan", order = 1)]
public class OmegaPlan : ScriptableObject
{
    [System.Serializable]
    public class OPObjective {

        public enum ObjectiveState
        {
            None,
            Incomplete,
            Complete,
        }
        public string m_name;
        public string m_description;
        public int m_cardID = -1;
        public ObjectiveState m_state = ObjectiveState.Incomplete;
    }
    public string m_name;
    public string m_description;
    public OPObjective[] m_objectives;

}

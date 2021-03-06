using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameState : Object
{
    public List<Event> m_events;
    public List<Item> m_currentItems = new List<Item>();
    public List<Stat> m_stats = new List<Stat>();
    public int m_turnNumber = 0;
    public Dictionary<string, Condition> m_globalVariables = new Dictionary<string, Condition>();
    public string m_nextEvent;
    public OmegaPlan m_omegaPlan;
   public void Initialize (List<Event> events, OmegaPlan op) {
       
        m_events = events;
        m_omegaPlan = Instantiate(op);

        Stat s1 = new Stat();
        s1.m_name = "Infamy";
        s1.m_currentValue = 50;
        m_stats.Add(s1);

        Stat s2 = new Stat();
        s2.m_name = "Loyalty";
        s2.m_currentValue = 50;
        m_stats.Add(s2);

        Stat s3 = new Stat();
        s3.m_name = "Influence";
        s3.m_currentValue = 50;
        m_stats.Add(s3);

        Stat s4 = new Stat();
        s4.m_name = "Money";
        s4.m_currentValue = 50;
        m_stats.Add(s4);
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameState
{
    public List<Event> m_events;
    public List<Stat> m_stats = new List<Stat>();
    public int m_turnNumber = 0;
    public Dictionary<string, Condition> m_globalVariables = new Dictionary<string, Condition>();
    public string m_nextEvent;
   public void Initialize (List<Event> events) {
       
        m_events = events;

        Stat s1 = new Stat();
        s1.m_name = "statOne";
        s1.m_currentValue = 50;
        m_stats.Add(s1);

        Stat s2 = new Stat();
        s2.m_name = "statTwo";
        s2.m_currentValue = 50;
        m_stats.Add(s2);

        Stat s3 = new Stat();
        s3.m_name = "statThree";
        s3.m_currentValue = 50;
        m_stats.Add(s3);

        Stat s4 = new Stat();
        s4.m_name = "statFour";
        s4.m_currentValue = 50;
        m_stats.Add(s4);
   }

   public void StartTurn () {
       m_turnNumber ++;
   }
}

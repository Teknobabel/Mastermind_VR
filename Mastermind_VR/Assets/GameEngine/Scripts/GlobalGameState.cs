using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameState
{
    public List<Event> m_events;
    public List<Stat> m_stats = new List<Stat>();

    public int m_turnNumber = 0;
   public void Initialize (List<Event> events) {
       
        m_events = events;

        Stat s1 = new Stat();
        s1.m_name = "Stat 1";
        s1.m_currentValue = 50;
        m_stats.Add(s1);

        Stat s2 = new Stat();
        s2.m_name = "Stat 2";
        s2.m_currentValue = 50;
        m_stats.Add(s2);

        Stat s3 = new Stat();
        s3.m_name = "Stat 3";
        s3.m_currentValue = 50;
        m_stats.Add(s3);

        Stat s4 = new Stat();
        s4.m_name = "Stat 4";
        s4.m_currentValue = 50;
        m_stats.Add(s4);
   }

   public void StartTurn () {
       m_turnNumber ++;
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager 
{

    public static Event GetEvent (GlobalGameState globalGameState) {

        List<Event> validEvents = new List<Event>();

        foreach(Event e in globalGameState.m_events)
        {
            if (globalGameState.m_turnNumber >= e.m_lockTurn)
            {
                int weight = e.m_weight+1;
                for (int i=0; i<weight; i++)
                {
                    validEvents.Add(e);
                }
            }
        }

        Event newEvent = validEvents[Random.Range(0, validEvents.Count)];
        return newEvent;
    }
   public static List<Event> BuildEvents (List<List<string>> eventData) {
            
            List<Event> events = new List<Event>();
            foreach(List<string> l in eventData)
            {
                Event e = new Event();

                e.m_name = l[0];
                e.m_id = int.Parse(l[1]);
                e.m_bearer = l[2];

                if (l[4] != "")
                {
                    e.m_lockTurn = int.Parse(l[4]);
                }

                if (l[5] != "")
                {
                    e.m_weight = int.Parse(l[5]);
                }
                
                e.m_question = l[6];

                if (l[7] == "")
                {
                    e.m_overrideYes = "Yes";
                } else {
                    e.m_overrideYes = l[7];
                }

                if (l[8] != "")
                {
                    e.m_answerYes = l[8];
                }

                if (l[8] == "")
                {
                    e.m_overrideNo = "No";
                } else {
                    e.m_overrideNo = l[8];
                }

                if (l[15] != "")
                {
                    e.m_answerNo = l[15];
                }

                if (l[9] != "")
                {
                    e.m_yesStat1 = int.Parse(l[9]);
                }

                if (l[10] != "")
                {
                    e.m_yesStat2 = int.Parse(l[10]);
                }

                if (l[11] != "")
                {
                    e.m_yesStat3 = int.Parse(l[11]);
                }

                if (l[12] != "")
                {
                    e.m_yesStat4 = int.Parse(l[12]);
                }

                if (l[16] != "")
                {
                    e.m_noStat1 = int.Parse(l[16]);
                }

                if (l[17] != "")
                {
                    e.m_noStat2 = int.Parse(l[17]);
                }

                if (l[18] != "")
                {
                    e.m_noStat3 = int.Parse(l[18]);
                }

                if (l[19] != "")
                {
                    e.m_noStat4 = int.Parse(l[19]);
                }
                events.Add(e);
            }

            return events;
        }
}

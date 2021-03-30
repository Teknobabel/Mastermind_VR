using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager 
{

    public static Event GetEvent (GlobalGameState globalGameState) {

        if (globalGameState.m_nextEvent != null)
        {
            foreach(Event e in globalGameState.m_events)
            {
                if (globalGameState.m_nextEvent == e.m_name)
                {
                    Event nextEvent = e;
                    globalGameState.m_nextEvent = null;
                    return nextEvent;
                }
            }
            
        }

        List<Event> validEvents = new List<Event>();

        foreach(Event e in globalGameState.m_events)
        {
            if (globalGameState.m_turnNumber >= e.m_turnAvailable && e.m_lockedTurnsRemaining == 0)
            {

                //Check conditions
                bool conditionsMet = true;
               // Debug.Log(e.m_conditions.Count);
                foreach(KeyValuePair<string, Condition> entry in e.m_conditions)
                {
                    // boolean value condition
                    if (globalGameState.m_globalVariables.ContainsKey(entry.Key)) {
                        
                        Condition c = globalGameState.m_globalVariables[entry.Key];
                       // Debug.Log(entry.Value.value);
                       // Debug.Log(entry.Value.operation);
                       // Debug.Log(c.value);
                        if (entry.Value.operation == Condition.Operation.Boolean && 
                            entry.Value.value != c.value)
                        {
                            conditionsMet = false;
                            //Debug.Log("Event conditions not met: " + entry.Key);
                            break;
                        }
                    } else {

                        // integer condition (check against current stat value)

                        foreach (Stat s in globalGameState.m_stats)
                        {
                            if (entry.Key == s.m_name)
                            {
                                //Debug.Log(s.m_currentValue);
                                //Debug.Log(entry.Value.value);
                                if (entry.Value.operation == Condition.Operation.GreaterThan && 
                                    s.m_currentValue < entry.Value.value)
                                {
                                    conditionsMet = false;
                                        //Debug.Log("Event conditions not met: " + entry.Key);
                                        break;

                                } else if (entry.Value.operation == Condition.Operation.LessThan &&
                                    s.m_currentValue > entry.Value.value)
                                    {

                                        conditionsMet = false;
                                        //Debug.Log("Event conditions not met: " + entry.Key);
                                        break;
                                }
                            }
                        }
                    }
                }

                if (conditionsMet)
                {
                    int weight = e.m_weight+1;
                    for (int i=0; i<weight; i++)
                    {
                        validEvents.Add(e);
                    }
                }
            }
        }

        Event newEvent = validEvents[Random.Range(0, validEvents.Count)];
        return newEvent;
    }
   public static List<Event> BuildEvents (List<List<string>> eventData, GlobalGameState globalGameState) {
            
            int cardName = 0;
            int cardID = 1;
            int bearer = 2;
            int conditions = 3;
            int turnAvailable = 4;
            int weight = 5;
            int question = 6;
            int overrideYes = 7;
            int answerYes = 8;
            int yInfamy = 9;
            int yLoyalty = 10;
            int yInfluence = 11;
            int yMoney = 12;
            int yCustom = 13;
            int overrideNo = 14;
            int answerNo = 15;
            int nInfamy = 16;
            int nLoyalty = 17;
            int nInfluence = 18;
            int nMoney = 19;
            int nCustom = 20;
            int turnsLocked = 21;


            List<Event> events = new List<Event>();
            foreach(List<string> l in eventData)
            {
                Event e = new Event();

                e.m_name = l[cardName];
                e.m_id = int.Parse(l[cardID]);
                e.m_bearer = l[bearer];
                e.m_lockedTurnsRemaining = 0;

                if (l[turnAvailable] != "")
                {
                    e.m_turnAvailable = int.Parse(l[turnAvailable]);
                }

                if (l[weight] != "")
                {
                    e.m_weight = int.Parse(l[weight]);
                }
                
                e.m_question = l[question];

                if (l[overrideYes] == "")
                {
                    e.m_overrideYes = "Yes";
                } else {
                    e.m_overrideYes = l[overrideYes];
                }

                if (l[answerYes] != "")
                {
                    e.m_answerYes = l[answerYes];
                }

                if (l[overrideNo] == "")
                {
                    e.m_overrideNo = "No";
                } else {
                    e.m_overrideNo = l[overrideNo];
                }

                if (l[answerNo] != "")
                {
                    e.m_answerNo = l[answerNo];
                }

                if (l[yInfamy] != "")
                {
                    e.m_yesStat1 = int.Parse(l[yInfamy]);
                }

                if (l[yLoyalty] != "")
                {
                    e.m_yesStat2 = int.Parse(l[yLoyalty]);
                }

                if (l[yInfluence] != "")
                {
                    e.m_yesStat3 = int.Parse(l[yInfluence]);
                }

                if (l[yMoney] != "")
                {
                    e.m_yesStat4 = int.Parse(l[yMoney]);
                }

                if (l[nInfamy] != "")
                {
                    e.m_noStat1 = int.Parse(l[nInfamy]);
                }

                if (l[nLoyalty] != "")
                {
                    e.m_noStat2 = int.Parse(l[nLoyalty]);
                }

                if (l[nInfluence] != "")
                {
                    e.m_noStat3 = int.Parse(l[nInfluence]);
                }

                if (l[nMoney] != "")
                {
                    e.m_noStat4 = int.Parse(l[nMoney]);
                }

                if (l[turnsLocked] == "")
                {
                    e.m_turnsLocked = 0;
                } else {
                    e.m_turnsLocked = int.Parse(l[turnsLocked]);
                }

                // parse all the condition text

                e.m_conditions = new Dictionary<string, Condition>();

                if (l[conditions] != "")
                {
                    // split line into individual conditions
                    string baseString = l[conditions];
                    string[] splitString = baseString.Split();
                    // foreach(string st in splitS)
                    // {
                    //     Debug.Log(st);
                    // }
                    
                    

                    for (int j=0; j<splitString.Length; j++)
                    {
                        string s = splitString[j];
                        Condition c = new Condition();

                        //int value

                        if (s.IndexOf('<') != -1 || s.IndexOf('>') != -1 || s.IndexOf('=') != -1) {

                        string num = string.Empty;
                        string value = string.Empty;

                        for (int i=0; i< s.Length; i++)
                        {
                            if (System.Char.IsDigit(s[i]))
                            {
                                num += s[i];
                            } else if (s[i] == '<') 
                            {
                                c.operation = Condition.Operation.LessThan;

                            } else if (s[i] == '>') {

                                c.operation = Condition.Operation.GreaterThan;

                            } else if (s[i] == '=') {

                                c.operation = Condition.Operation.EqualTo;
                            } else {
                                value += s[i];
                            }
                        }

                        int intValue = int.Parse(num);
                        c.value = intValue;
                        e.m_conditions.Add(value, c);
                        Debug.Log(value + " " + c.operation + " " + intValue);

                        } else {

                            //bool value
                            Condition c2 = new Condition();
                            c2.operation = Condition.Operation.Boolean;
                            int value = 1;
                            if (s[0] == '!')
                            {
                                value = 0;
                                string tempStr = s;
                                s = tempStr.Remove(0, 1);
                            }

                            Debug.Log(s + " " + c2.operation + " " + value);
                            c2.value = value;
                            e.m_conditions.Add(s, c2);

                            if (!globalGameState.m_globalVariables.ContainsKey(s))
                            {
                                Condition newGlobalVariable = new Condition();
                                newGlobalVariable.value = 0;
                                newGlobalVariable.operation = Condition.Operation.Boolean;
                                globalGameState.m_globalVariables.Add(s, newGlobalVariable);
                                Debug.Log("Adding new global variable: " + s + " " + newGlobalVariable.operation + " " + newGlobalVariable.value);
                            }
                        }
                    }
                    
                }

                 // Yes actions
                    
                    e.m_yesCustom = new Dictionary<string, Condition>();

                    if (l[yCustom] != "")
                    {
                        string baseString = l[yCustom];
                        string[] splitString = baseString.Split();

                        for (int j=0; j<splitString.Length; j++)
                        {
                            string s = splitString[j];
                            //string s = l[yCustom];
                            Condition yesCustom = new Condition();
                            yesCustom.operation = Condition.Operation.Boolean;
                            int value = 1;
                            if (s[0] == '+')
                            {
                                yesCustom.operation = Condition.Operation.GainItem;
                            }
                            else if (s[0] == '>')
                            {
                                yesCustom.operation = Condition.Operation.LoadEvent;
                                string tempStr = s;
                                s = tempStr.Remove(0, 1);
                            }
                            else if (s[0] == '!')
                            {
                                value = 0;
                                string tempStr = s;
                                s = tempStr.Remove(0, 1);
                            }

                            Debug.Log("Adding Yes Custom: " + s + " " + yesCustom.operation + " " + value);
                            yesCustom.value = value;
                            e.m_yesCustom.Add(s, yesCustom);
                        }
                    }

                    // No actions

                    e.m_noCustom = new Dictionary<string, Condition>();
                    
                    if (l[nCustom] != "")
                    {
                        string baseString = l[nCustom];
                        string[] splitString = baseString.Split();

                        for (int j=0; j<splitString.Length; j++)
                        {
                            string s = splitString[j];
                            //string s = l[nCustom];
                            Condition noCustom = new Condition();
                            noCustom.operation = Condition.Operation.Boolean;
                            int value = 1;

                            if (s[0] == '+')
                            {
                                noCustom.operation = Condition.Operation.GainItem;
                            }
                            else if (s[0] == '>')
                            {
                                noCustom.operation = Condition.Operation.LoadEvent;
                                string tempStr = s;
                                s = tempStr.Remove(0, 1);

                            }
                            else if (s[0] == '!')
                            {
                                value = 0;
                                string tempStr = s;
                                s = tempStr.Remove(0, 1);
                            }

                            Debug.Log("Adding No Custom: " + s + " " + noCustom.operation + " " + value);
                            noCustom.value = value;
                            e.m_noCustom.Add(s, noCustom);
                        }
                    }

                events.Add(e);
            }

            return events;
        }

        
}

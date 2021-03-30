using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum AnswerType {
        None,
        Yes,
        No,
    }
    public OmegaPlan m_omegaPlan;
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    public List<List<string>> m_eventData;
    private int m_playerChoice = 0;
    public int playerChoice {get{return m_playerChoice;} set{m_playerChoice = value;}}
    private GlobalGameState m_globalGameState;
    public GlobalGameState globalGameState {get{return m_globalGameState;} set{m_globalGameState = value;}}
    private bool m_gameOver = false;
    private AnswerType m_answer = AnswerType.None;
    public AnswerType answer {get{return m_answer;}}
    
    void Awake () {

        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // m_eventManager = new EventManager();
        StartCoroutine(InitializeGame());
        
    }

    public IEnumerator InitializeGame ()
    {
        Debug.Log("Initializing Game");
        yield return StartCoroutine (GoogleSheetDownload.GetEventData(this));
        m_globalGameState = new GlobalGameState();
        m_globalGameState.Initialize(EventManager.BuildEvents(m_eventData, m_globalGameState), m_omegaPlan);
        m_eventData = null;
        GameView.Instance.Initialize(m_globalGameState);
        StartCoroutine(DoTurn());
        yield return null;
    }

    public IEnumerator DoTurn () {

        m_globalGameState.StartTurn();

        Event newEvent = EventManager.GetEvent(m_globalGameState);

        GameView.Instance.DisplayEvent(newEvent);

        yield return StartCoroutine (InputManager.GetInput());

        ProcessChoise(newEvent);

        if (m_answer != AnswerType.None)
        {
            GameView.Instance.DisplayAnswer(newEvent);
            yield return StartCoroutine (InputManager.GetInput());
        }

        CheckForGameOver();

        if (!m_gameOver) {
            EndTurn();
            StartCoroutine(DoTurn());
        } else {
            GameView.Instance.DisplayGameOver();
            yield return StartCoroutine (InputManager.GetInput());
        }
        yield return null;
    }

    private void EndTurn () {
        m_playerChoice = 0;
        m_answer = AnswerType.None;

        for (int i=0; i<m_globalGameState.m_events.Count; i++)
        {
            Event e = m_globalGameState.m_events[i];
            
            if (e.m_lockedTurnsRemaining > 0) {
                e.m_lockedTurnsRemaining -= 1;
                //Debug.Log(e.m_lockedTurnsRemaining);
            }
        }
    }

    // public void DisplayEvent (Event thisEvent) {

    //     Debug.Log("----------------------------------------------------------");
    //     Debug.Log("Turn " + m_globalGameState.m_turnNumber);
    //     string currentStats = "Stats:";
    //     foreach (Stat s in m_globalGameState.m_stats)
    //     {
    //         currentStats += " " + s.m_name + ": " + s.m_currentValue.ToString();
    //     }
        
    //     Debug.Log(currentStats);
    //     Debug.Log(thisEvent.m_bearer);
    //     Debug.Log(thisEvent.m_question);
    //     Debug.Log("1: " + thisEvent.m_overrideYes);
    //     Debug.Log("2: " + thisEvent.m_overrideNo);
    //     Debug.Log("----------------------------------------------------------");
    // }

    private void ProcessChoise (Event thisEvent)
    {
        Debug.Log("Processing Choice");
        

        switch (m_playerChoice)
        {
            case 1:
                int y1 = thisEvent.m_yesStat1;
                int y2 = thisEvent.m_yesStat2;
                int y3 = thisEvent.m_yesStat3;
                int y4 = thisEvent.m_yesStat4;
                
                m_globalGameState.m_stats[0].m_currentValue += y1;
                m_globalGameState.m_stats[1].m_currentValue += y2;
                m_globalGameState.m_stats[2].m_currentValue += y3;
                m_globalGameState.m_stats[3].m_currentValue += y4;

                if (thisEvent.m_answerYes != null)
                {
                    m_answer = AnswerType.Yes;
                }

                //process any yes outcomes

                if (thisEvent.m_yesCustom.Count > 0)
                {
                    foreach(KeyValuePair<string, Condition> entry in thisEvent.m_yesCustom)
                    {
                        if (entry.Value.operation == Condition.Operation.LoadEvent)
                        {
                            m_globalGameState.m_nextEvent = entry.Key;
                        }
                        else if (entry.Key == "OP1")
                        {
                            CompleteOPObjective(0);
                            
                        } else if (entry.Key == "OP2")
                        {
                            CompleteOPObjective(1);
                        } else if (entry.Key == "OP3")
                        {
                            CompleteOPObjective(2);
                        }
                        else if (m_globalGameState.m_globalVariables.ContainsKey(entry.Key) &&
                        m_globalGameState.m_globalVariables[entry.Key].value != entry.Value.value)
                        {
                            Condition c = m_globalGameState.m_globalVariables[entry.Key];
                            c.value = entry.Value.value;
                            Debug.Log("Setting global variable: " + entry.Key + " to " + c.value);
                            m_globalGameState.m_globalVariables[entry.Key] = c;
                        }
                    }
                }


            break;
            case 2:
                int n1 = thisEvent.m_noStat1;
                int n2 = thisEvent.m_noStat2;
                int n3 = thisEvent.m_noStat3;
                int n4 = thisEvent.m_noStat4;
                
                m_globalGameState.m_stats[0].m_currentValue += n1;
                m_globalGameState.m_stats[1].m_currentValue += n2;
                m_globalGameState.m_stats[2].m_currentValue += n3;
                m_globalGameState.m_stats[3].m_currentValue += n4;

                if (thisEvent.m_answerNo != null)
                {
                    m_answer = AnswerType.No;
                }

                //process any no outcomes

                if (thisEvent.m_noCustom.Count > 0)
                {
                    foreach(KeyValuePair<string, Condition> entry in thisEvent.m_noCustom)
                    {
                        if (entry.Value.operation == Condition.Operation.LoadEvent)
                        {
                            m_globalGameState.m_nextEvent = entry.Key;
                        }
                        else if (entry.Key == "OP1")
                        {
                            CompleteOPObjective(0);
                            
                        } else if (entry.Key == "OP2")
                        {
                            CompleteOPObjective(1);
                        } else if (entry.Key == "OP3")
                        {
                            CompleteOPObjective(2);
                        }
                        else if (m_globalGameState.m_globalVariables.ContainsKey(entry.Key) &&
                        m_globalGameState.m_globalVariables[entry.Key].value != entry.Value.value)
                        {
                            Condition c = m_globalGameState.m_globalVariables[entry.Key];
                            c.value = entry.Value.value;
                            Debug.Log("Setting global variable: " + entry.Key + " to " + c.value);
                            m_globalGameState.m_globalVariables[entry.Key] = c;
                        }
                    }
                }

            break;
        }
        m_playerChoice = 0;
        thisEvent.m_lockedTurnsRemaining = thisEvent.m_turnsLocked;
        
    }

    private void CompleteOPObjective (int objNumber)
    {
       OmegaPlan.OPObjective op = m_globalGameState.m_omegaPlan.m_objectives[objNumber];

       if (op.m_state != OmegaPlan.OPObjective.ObjectiveState.Complete)
       {
           op.m_state = OmegaPlan.OPObjective.ObjectiveState.Complete;
           GameView.Instance.DisplayOmegaPlan(m_globalGameState.m_omegaPlan);
       }
    }

    private void CheckForGameOver ()
    {
        foreach(Stat s in m_globalGameState.m_stats){

            if (s.m_currentValue <= s.m_minValue || s.m_currentValue >= s.m_maxValue) {
                Debug.Log("Game Over");
                m_gameOver = true;
            }
        }
    }
   
}

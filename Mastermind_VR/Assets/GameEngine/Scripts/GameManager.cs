using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    public List<List<string>> m_eventData;

    public int m_playerChoice = 0;
    private GlobalGameState m_globalGameState;
    private bool m_gameOver = false;
    
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
        m_globalGameState.Initialize(EventManager.BuildEvents(m_eventData));
        m_eventData = null;
        StartCoroutine(DoTurn());
        yield return null;
    }

    public IEnumerator DoTurn () {

        m_globalGameState.StartTurn();
        Event newEvent = EventManager.GetEvent(m_globalGameState);
        DisplayEvent(newEvent);
        yield return StartCoroutine (InputManager.GetInput());
        ProcessChoise(newEvent);
        CheckForGameOver();
        if (!m_gameOver) {
            StartCoroutine(DoTurn());
        }
        yield return null;
    }

    public void DisplayEvent (Event thisEvent) {

        Debug.Log("----------------------------------------------------------");
        Debug.Log("Turn " + m_globalGameState.m_turnNumber);
        string currentStats = "Stats:";
        foreach (Stat s in m_globalGameState.m_stats)
        {
            currentStats += " " + s.m_name + ": " + s.m_currentValue.ToString();
        }
        
        Debug.Log(currentStats);
        Debug.Log(thisEvent.m_bearer);
        Debug.Log(thisEvent.m_question);
        Debug.Log("1: " + thisEvent.m_overrideYes);
        Debug.Log("2: " + thisEvent.m_overrideNo);
        Debug.Log("----------------------------------------------------------");
    }

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
            break;
        }


        m_playerChoice = 0;
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

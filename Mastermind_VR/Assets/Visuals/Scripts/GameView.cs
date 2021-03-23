using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameView : MonoBehaviour
{
     [System.Serializable]
    public struct Portrait {
        public string m_name;
        public Texture m_sprite;
    }
    private static GameView _instance;
    public static GameView Instance { get { return _instance; } }
    public Text[] m_buttonText;
    public Button[] m_buttons;
    public Text m_questionText;
    public Text m_turnText;
    public StatUI[] m_stats;
    public RawImage m_portrait;
    public Portrait[] m_portraits;

    private Event m_currentEvent;
    void Awake () {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // public void Initialize (GlobalGameState globalGameState) {
        
        
    // }

    // Update is called once per frame
    public void DisplayEvent(Event thisEvent)
    {
        m_currentEvent = thisEvent;
        GlobalGameState g = GameManager.Instance.globalGameState;
        m_questionText.text = thisEvent.m_question;
        m_turnText.text = "Turn: " + g.m_turnNumber;
        m_buttonText[0].text = thisEvent.m_overrideYes;
        m_buttonText[1].text = thisEvent.m_overrideNo;

        m_buttons[0].gameObject.SetActive(true);
        m_buttons[1].gameObject.SetActive(true);
        m_buttons[2].gameObject.SetActive(false);

        DisplayStats();

        string s = thisEvent.m_bearer;
        foreach (Portrait p in m_portraits) {

            if (p.m_name == s) {
                m_portrait.texture = p.m_sprite;
                break;
            }
        }
    }

    public void DisplayAnswer (Event thisEvent)
    {
        DisplayStats();

        m_buttons[0].gameObject.SetActive(false);
        m_buttons[1].gameObject.SetActive(false);
        m_buttons[2].gameObject.SetActive(true);

        if (GameManager.Instance.answer == GameManager.AnswerType.Yes)
        {
            m_buttonText[2].text = thisEvent.m_answerYes;
        } else {
            m_buttonText[2].text = thisEvent.m_answerNo;
        }
    }

    public void DisplayGameOver () {
        
        m_portrait.texture = null;
        m_questionText.gameObject.SetActive(false);
        m_buttons[0].gameObject.SetActive(false);
        m_buttons[1].gameObject.SetActive(false);
        m_buttons[2].gameObject.SetActive(true);

        m_buttonText[2].text = "Game Over";

    }

    private void DisplayStats () {
        GlobalGameState g = GameManager.Instance.globalGameState;
        for (int i=0; i< g.m_stats.Count; i++)
        {
            Stat stat = g.m_stats[i];
            StatUI statUI = m_stats[i];
            statUI.ChangeMovementIndicatorState(false);
            float currentValue = (float)stat.m_currentValue;
            float maxValue = (float)stat.m_maxValue;
            float fillAmount = currentValue / maxValue;
            statUI.m_fill.fillAmount = fillAmount;
        }
    }

    

    private void ShowMovementIndicators (int buttonNum){
        int s1 = 0;
        int s2 = 0;
        int s3 = 0;
        int s4 = 0;

        if (buttonNum == 1){
            s1 = m_currentEvent.m_yesStat1;
            s2 = m_currentEvent.m_yesStat2;
            s3 = m_currentEvent.m_yesStat3;
            s4 = m_currentEvent.m_yesStat4;
        } else if (buttonNum == 2)
        {
            s1 = m_currentEvent.m_noStat1;
            s2 = m_currentEvent.m_noStat2;
            s3 = m_currentEvent.m_noStat3;
            s4 = m_currentEvent.m_noStat4;
        }

        if (s1 != 0){m_stats[0].ChangeMovementIndicatorState(true);}
        if (s2 != 0){m_stats[1].ChangeMovementIndicatorState(true);}
        if (s3 != 0){m_stats[2].ChangeMovementIndicatorState(true);}
        if (s4 != 0){m_stats[3].ChangeMovementIndicatorState(true);}
    }

    private void HideMovementIndicators () {

        foreach (StatUI s in m_stats){
            s.ChangeMovementIndicatorState(false);
        }

    }

    public void ButtonClick (int buttonNum)
    {
        GameManager.Instance.playerChoice = buttonNum;
    }

    public void ButtonHovered (int buttonNum)
    {
        ShowMovementIndicators(buttonNum);
    }

    public void EndButtonHOver (int buttonNum)
    {
        HideMovementIndicators();
    }
}

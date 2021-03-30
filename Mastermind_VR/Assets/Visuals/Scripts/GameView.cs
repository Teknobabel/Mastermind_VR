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
    public Text m_opName;
    public Text m_opDescription;
    public GameObject m_opObjectiveUI;
    public Transform m_opObjectiveList;

    private Event m_currentEvent;
    void Awake () {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize (GlobalGameState globalGameState) {
        
        DisplayOmegaPlan(globalGameState.m_omegaPlan);
    }

    // Update is called once per frame
    public void DisplayEvent(Event thisEvent)
    {
        m_currentEvent = thisEvent;
        GlobalGameState g = GameManager.Instance.globalGameState;
        m_questionText.text = thisEvent.m_bearer + ": " + thisEvent.m_question;
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

        m_buttonText[2].text = "Next";

        if (GameManager.Instance.answer == GameManager.AnswerType.Yes)
        {
            m_questionText.text = thisEvent.m_answerYes;
        } else {
            m_questionText.text = thisEvent.m_answerNo;
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
            statUI.m_nameText.text = stat.m_name;
        }
    }

    public void DisplayOmegaPlan (OmegaPlan op){

        m_opName.text = op.m_name;
        m_opDescription.text = op.m_description;

        List<GameObject> go = new List<GameObject>();

        foreach (Transform child in m_opObjectiveList)
        {
            go.Add(child.gameObject);
        }

        while (go.Count > 0)
        {
            GameObject g = go[0];
            go.RemoveAt(0);
            Destroy(g);
        }

        for (int i=0; i< op.m_objectives.Length; i++)
        {
            OmegaPlan.OPObjective obj = op.m_objectives[i];
            OPObjectiveUI obUI = (OPObjectiveUI) Instantiate(m_opObjectiveUI, m_opObjectiveList).GetComponent<OPObjectiveUI>();
            
            if (obj.m_state == OmegaPlan.OPObjective.ObjectiveState.Complete)
            {
                obUI.m_objNumber.text = "C";
                obUI.m_background.color = Color.green;
            } else {
                obUI.m_objNumber.text = (i+1).ToString();
            }
            obUI.m_objName.text = obj.m_name;
            obUI.m_objDescription.text = obj.m_description;
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

        if (s1 != 0){

            if (s1 >= 15)
            {
                m_stats[0].ChangeLargeMovementIndicatorState(true);
            } else {
                m_stats[0].ChangeMovementIndicatorState(true);
            }
        }

        if (s2 != 0){

            if (s2 >= 15) {
                m_stats[1].ChangeLargeMovementIndicatorState(true);
            } else {
                m_stats[1].ChangeMovementIndicatorState(true);
            }
        }

        if (s3 != 0){

            if (s3 >= 15) {
                m_stats[2].ChangeLargeMovementIndicatorState(true);
            } else {
                m_stats[2].ChangeMovementIndicatorState(true);
            }
        }

        if (s4 != 0){

            if (s4 >= 15) {
                m_stats[3].ChangeLargeMovementIndicatorState(true);
            } else {
                m_stats[3].ChangeMovementIndicatorState(true);
            }
        }
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

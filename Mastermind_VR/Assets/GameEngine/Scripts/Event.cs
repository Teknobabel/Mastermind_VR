using System.Collections.Generic;
public class Event 
{
    public string m_name;
    public string m_bearer;
    public string m_question;
    public string m_overrideYes;
    public string m_answerYes;
    public string m_overrideNo;
    public string m_answerNo;
    public int m_id;
    public int m_turnAvailable;
    public int m_weight;
    public int m_yesStat1;
    public int m_yesStat2;
    public int m_yesStat3;
    public int m_yesStat4;
    public int m_noStat1;
    public int m_noStat2;
    public int m_noStat3;
    public int m_noStat4;
    public int m_lockedTurnsRemaining;
    public int m_turnsLocked;
    public Dictionary<string, Condition> m_conditions;
    public Dictionary<string, Condition> m_yesCustom;
    public Dictionary<string, Condition> m_noCustom;
    
}

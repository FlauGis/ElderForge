using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptQuestKillWolf : MonoBehaviour
{
    public GameObject quest;
    public bool questDone = false;
    public bool questTake = false;
    public bool questFineshed = false;
    public GameObject miniMap;
    public void TakeQuest()
    {
        quest.SetActive(true);
        questTake = true;
    }

    public void FinishQuest()
    {
        questFineshed = true;
        miniMap.SetActive(true);
    }
}

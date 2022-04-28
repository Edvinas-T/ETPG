using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    Manager instance;
    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        CHECKALIVE,
        WIN,
        LOSE
    }
    public PerformAction battleStates;

    public List<HandleTurns> PerformList = new List<HandleTurns>();
    public List<GameObject> PlayersInBattle = new List<GameObject>();
    public List<GameObject> EnemiesinBattle = new List<GameObject>();

    public enum HeroUI
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }

    public HeroUI HeroInput;

    public List<GameObject> HeroesToManage = new List<GameObject>();
    private HandleTurns HeroChoice;

    public GameObject enemyButton;
    public Transform Spacer;

    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;

    private List<GameObject> enemyBtns = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        battleStates = PerformAction.WAIT;
        EnemiesinBattle.AddRange(GameObject.FindGameObjectsWithTag("NPC"));
        PlayersInBattle.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        HeroInput = HeroUI.ACTIVATE;

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);

        EnemyButtons();
    }

    // Update is called once per frame
    void Update()
    {
        switch (battleStates)
        {
            case (PerformAction.WAIT):
                if (PerformList.Count > 0)
                {
                    battleStates = PerformAction.TAKEACTION;
                }

                break;

            case (PerformAction.TAKEACTION):
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if(PerformList[0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.heroToAttack = PerformList[0].AttackersTarget;
                    ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                }
                if (PerformList[0].Type == "Hero")
                {
                    HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
                    HSM.EnemytoAttack = PerformList[0].AttackersTarget;
                    HSM.currentState = HeroStateMachine.TurnState.ACTION;
                }
                battleStates = PerformAction.PERFORMACTION;
                break;

            case (PerformAction.PERFORMACTION):
                //just an idlestate
                break;

            case (PerformAction.CHECKALIVE):
                if(PlayersInBattle.Count < 1)
                {
                    battleStates = PerformAction.LOSE;
                }
                else if(EnemiesinBattle.Count < 1)
                {
                    battleStates = PerformAction.WIN;
                }
                else
                {
                    clearAttackPanel();
                    HeroInput = HeroUI.ACTIVATE;
                }
                break;

            case (PerformAction.LOSE):
                {
                    Debug.Log("Lost game");
                    for (int i = 0; i < EnemiesinBattle.Count; i++)
                    {
                        EnemiesinBattle[i].GetComponent<EnemyStateMachine>().currentState = EnemyStateMachine.TurnState.WAITING;
                    }
                    SceneManager.LoadScene(0);
                }
               
                break;

            case (PerformAction.WIN):
                {
                    Debug.Log("You won");
                    for (int i = 0; i < PlayersInBattle.Count; i++)
                    {
                        PlayersInBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.WAITING;
                    }
                    SceneManager.LoadScene(0);
                }
                break;

    }

        switch (HeroInput)
        {
            case (HeroUI.ACTIVATE):
                if(HeroesToManage.Count > 0)
                {
                    HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                    HeroChoice = new HandleTurns();
                    AttackPanel.SetActive(true);
                    HeroInput = HeroUI.WAITING;
                }
            break;

            case (HeroUI.WAITING):

                break;

            case (HeroUI.DONE):
                HeroInputDone();
                break;
        }

    }

    public void CollectActions(HandleTurns input)
    {
        PerformList.Add(input);
    }
    public void EnemyButtons()
    {
        foreach(GameObject enemyBtn in enemyBtns)
        {
            Destroy(enemyBtn);
        }
        enemyBtns.Clear();

        foreach(GameObject enemy in EnemiesinBattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyStateMachine curEnemy = enemy.GetComponent<EnemyStateMachine>();

            Text buttonText = newButton.GetComponentInChildren<Text>();
            buttonText.text = curEnemy.enemy.theName;

            button.EnemyPrefab = enemy;

            newButton.transform.SetParent(Spacer,false);
            enemyBtns.Add(newButton);
        }
    }
    
    public void Attack() 
    {
        HeroChoice.Attacker = HeroesToManage[0].name;
        HeroChoice.AttackersGameObject = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        HeroChoice.chooseAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().player.Attacks[0];

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }
    public void SelectEnemy(GameObject chooseEnemy)
    {
        HeroChoice.AttackersTarget = chooseEnemy;
        HeroInput = HeroUI.DONE;
    }

    public void HeroInputDone()
    {
        PerformList.Add(HeroChoice);
        clearAttackPanel();
        
        HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        HeroesToManage.RemoveAt(0);
        HeroInput = HeroUI.ACTIVATE;
    }

    void clearAttackPanel()
    {
        EnemySelectPanel.SetActive(false);
        AttackPanel.SetActive(false);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour { 

    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION
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

    // Start is called before the first frame update
    void Start()
    {
        battleStates = PerformAction.WAIT;
        EnemiesinBattle.AddRange(GameObject.FindGameObjectsWithTag("NPC"));
        PlayersInBattle.AddRange(GameObject.FindGameObjectsWithTag("Player"));

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
                if (PerformList[0].Type == "Player")
                {

                }
                battleStates = PerformAction.PERFORMACTION;
                break;

            case (PerformAction.PERFORMACTION):
                break;
        }
    }

    public void CollectActions(HandleTurns input)
    {
        PerformList.Add(input);
    }
    void EnemyButtons()
    {
        foreach(GameObject enemy in EnemiesinBattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyStateMachine curEnemy = enemy.GetComponent<EnemyStateMachine>();

            Text buttonText = newButton.GetComponentInChildren<Text>();
            buttonText.text = curEnemy.enemy.name;

            button.EnemyPrefab = enemy;

            newButton.transform.SetParent(Spacer,false);
        }
    }
}

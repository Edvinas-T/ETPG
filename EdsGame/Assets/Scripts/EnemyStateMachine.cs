using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private BattleManager BM;
    public BattleEnemy enemy;

    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }
    public TurnState currentState;

    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;

    private Vector3 startpos;
    // Start is called before the first frame update
    void Start()
    {
        currentState = TurnState.PROCESSING;
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        startpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                UpdateProgressBar();
                break;

            case (TurnState.CHOOSEACTION):
                ChooseAction();
                currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):

                break;

            case (TurnState.ACTION):

                break;

            case (TurnState.DEAD):

                break;

        }
        void UpdateProgressBar()
        {
            cur_cooldown = cur_cooldown + Time.deltaTime;
         
            
            if (cur_cooldown >= max_cooldown)
            {
                currentState = TurnState.CHOOSEACTION;
            }
        }

        void ChooseAction()
        {
            HandleTurns myAttack = new HandleTurns();
            myAttack.Attacker = enemy.name;
            myAttack.AttackersGameObject = this.gameObject;
            myAttack.AttackersTarget = BM.PlayersInBattle[Random.Range(0, BM.PlayersInBattle.Count)];
            BM.CollectActions(myAttack);
        }
    }
}

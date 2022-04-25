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

    private bool actionStarted = false;
    public GameObject heroToAttack;
    private float animSpeed = 5f;
    public Animator NPCAnimator;
    public GameObject Selector;

    void Start()
    {
        currentState = TurnState.PROCESSING;
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        startpos = transform.position;
        Selector.SetActive(false);
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
                StartCoroutine(TimeForAction());
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
            myAttack.Attacker = enemy.theName;
            myAttack.Type = "Enemy";
            myAttack.AttackersGameObject = this.gameObject;
            myAttack.AttackersTarget = BM.PlayersInBattle[Random.Range(0, BM.PlayersInBattle.Count)];
            BM.CollectActions(myAttack);
        }
       
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;

        //animate enemy to attack player
        Vector3 heroPos = new Vector3(heroToAttack.transform.position.x, heroToAttack.transform.position.y, heroToAttack.transform.position.z -4f);
        NPCAnimator.SetBool("isMoving", true);
        while (MoveTowardsEnemy(heroPos))
        {
            yield return null;
        }

        //wait
        NPCAnimator.SetBool("isAttack", true);
        yield return new WaitForSeconds(1.9f);
        NPCAnimator.SetBool("isAttack", false);
        //do damage

        //animate back to idle
        Vector3 firstPos = startpos;
        while (MoveTowardsStart(firstPos))
        {
            yield return null;
        }
        NPCAnimator.SetBool("isMoving", false);
        //remove performer from list
        BM.PerformList.RemoveAt(0);
        //reset bm -> wait
        BM.battleStates = BattleManager.PerformAction.WAIT;
        

        actionStarted = false;
        //reset this enemy state
        cur_cooldown = 0;
        currentState = TurnState.PROCESSING;

    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
}

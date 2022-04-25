using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour
{
    private BattleManager BM;
    public BattlePlayer player;
    
    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    public Image ProgressBar;
    public GameObject Selector;

    public GameObject EnemytoAttack;
    private bool actionStarted = false;
    private Vector3 startpos;
    private float animSpeed = 5f;
    public Animator NPCAnimator;


    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        cur_cooldown = Random.Range(0, player.agilty / max_cooldown);
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        currentState = TurnState.PROCESSING;

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

            case (TurnState.ADDTOLIST):
                BM.HeroesToManage.Add(this.gameObject);
                currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):
               //idle
                break;

            case (TurnState.SELECTING):
             
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
            float calc_cooldown = cur_cooldown / max_cooldown;
            ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), 
                ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);

                if (cur_cooldown >= max_cooldown)
            {
                currentState = TurnState.ADDTOLIST;
            }
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
        Vector3 enemyPos = new Vector3(EnemytoAttack.transform.position.x, EnemytoAttack.transform.position.y, EnemytoAttack.transform.position.z);
        NPCAnimator.SetBool("isMoving", true);
        while (MoveTowardsEnemy(enemyPos))
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
        cur_cooldown = 0f;
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

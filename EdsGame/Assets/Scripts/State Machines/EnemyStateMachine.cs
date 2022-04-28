using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private PanelStats stats;
    public GameObject EnemyPanel;
    private Image ProgressBar;

    private bool alive = true;

    void Start()
    {
        createEnemyPanel();
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
                if (!alive)
                {
                    return;
                }
                else
                {
                    this.gameObject.tag = "DeadEnemy";
                    BM.EnemiesinBattle.Remove(this.gameObject);
                    Selector.SetActive(false);
                    //removes all enemy inputs
                    for (int i = 0; i < BM.PerformList.Count; i++)
                    {
                        if (BM.PerformList[i].AttackersGameObject == this.gameObject)
                        {
                            BM.PerformList.Remove(BM.PerformList[i]);
                        }
                        
                    }
                    alive = false;
                    //resets select enemy btns
                    BM.EnemyButtons();
                    BM.battleStates = BattleManager.PerformAction.CHECKALIVE;

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

                    int num = Random.Range(0, enemy.Attacks.Count);
                    myAttack.chooseAttack = enemy.Attacks[num];
                    Debug.Log(this.gameObject.name + " has chosen " + myAttack.chooseAttack.attackName + " and does " + myAttack.chooseAttack.attackDamage + " damage!");

                    BM.CollectActions(myAttack);
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
        doDamage();
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

    void doDamage()
    {
        float calcDamage = enemy.curATK + BM.PerformList[0].chooseAttack.attackDamage;
        heroToAttack.GetComponent<HeroStateMachine>().takeDamage(calcDamage);
    }
    public void takeDamage(float damageAmount)
    {
        enemy.curHP -= damageAmount;
        if(enemy.curHP <= 0)
        {
            enemy.curHP = 0;
            currentState = TurnState.DEAD;
        }
        updateEnemyPanel();
    }
    void createEnemyPanel()
    {
        stats = EnemyPanel.GetComponent<PanelStats>();
        stats.HeroName.text = enemy.theName;
        stats.HeroHP.text = "HP: " + enemy.curHP;
        stats.HeroMP.text = "MP: " + enemy.curMP;
        ProgressBar = stats.ProgressBar;

    }
    void updateEnemyPanel()
    {
        stats.HeroHP.text = "HP: " + enemy.curHP;
        stats.HeroMP.text = "MP: " + enemy.curMP;

    }
}


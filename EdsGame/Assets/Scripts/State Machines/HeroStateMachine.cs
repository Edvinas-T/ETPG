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
    private Image ProgressBar;
    public GameObject Selector;

    public GameObject EnemytoAttack;
    private bool actionStarted = false;
    private Vector3 startpos;
    private float animSpeed = 5f;
    public Animator NPCAnimator;

    private bool alive = true;

    private PanelStats stats;
    public GameObject HeroPanel;
    


    // Start is called before the first frame update
    void Start()
    {
        createPlayerPanel();

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

            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                break;

            case (TurnState.DEAD):
                if(!alive)
                {
                    return;
                }
                else
                {
                    this.gameObject.tag = "DeadHero";
                    //enemy cant target
                    BM.PlayersInBattle.Remove(this.gameObject);
                    //user cant manage
                    BM.HeroesToManage.Remove(this.gameObject);

                    Selector.SetActive(false);

                    BM.AttackPanel.SetActive(false);
                    BM.EnemySelectPanel.SetActive(false);
                    //removes from perform list
                    for (int i = 0; i < BM.PerformList.Count; i++)
                    {
                        if(BM.PerformList[i].AttackersGameObject == this.gameObject)
                        {
                            BM.PerformList.Remove(BM.PerformList[i]);
                        }
                    }

                    BM.battleStates = BattleManager.PerformAction.CHECKALIVE;
                    alive = false;
                }
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
        Vector3 enemyPos = new Vector3(EnemytoAttack.transform.position.x,
            EnemytoAttack.transform.position.y, EnemytoAttack.transform.position.z +3f);
        NPCAnimator.SetBool("isMoving", true);
        while (MoveTowardsEnemy(enemyPos))
        {
            yield return null;
        }

        //wait
        NPCAnimator.SetBool("isAttack", true);
        yield return new WaitForSeconds(1f);
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
        if (BM.battleStates != BattleManager.PerformAction.WIN && BM.battleStates != BattleManager.PerformAction.LOSE)
        {
            BM.battleStates = BattleManager.PerformAction.WAIT;
        }
        else
        {
            currentState = TurnState.WAITING;
        }
        


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
    public void takeDamage(float damageAmount)
    {
        player.curHP -= damageAmount;
        if(player.curHP <= 0)
        {
            player.curHP = 0;
            currentState = TurnState.DEAD;
        }
        updatePlayerPanel();
    }
    void doDamage()
    {
        float calcDamage = player.curATK + player.strength + BM.PerformList[0].chooseAttack.attackDamage;
        EnemytoAttack.GetComponent<EnemyStateMachine>().takeDamage(calcDamage);
    }
    void createPlayerPanel()
    {
        stats = HeroPanel.GetComponent<PanelStats>();
        stats.HeroName.text = player.theName;
        stats.HeroHP.text = "HP: " + player.curHP;
        stats.HeroMP.text = "MP: " + player.curMP;
        ProgressBar = stats.ProgressBar;
        
    }
    void updatePlayerPanel()
    {
        stats.HeroHP.text = "HP: " + player.curHP;
        stats.HeroMP.text = "MP: " + player.curMP;
       
    }
}

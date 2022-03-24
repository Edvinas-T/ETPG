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
    // Start is called before the first frame update
    void Start()
    {
        BM = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        currentState = TurnState.PROCESSING;

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
}

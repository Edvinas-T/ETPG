using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectButton : MonoBehaviour
{
    public GameObject EnemyPrefab;


    public void SelectEnemy()
    {
        GameObject.Find("BattleManager").GetComponent<BattleManager>().SelectEnemy(EnemyPrefab); // saves enemy input prefab
    }

    public void hideSelector()
    {
        
            EnemyPrefab.transform.FindChild("Selector").gameObject.SetActive(false);
        
       
    }
    public void showSelector()
    {

        EnemyPrefab.transform.FindChild("Selector").gameObject.SetActive(true);


    }

}

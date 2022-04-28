using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
   
    public static Manager manager;
    public GameObject player;

    public Vector3 playerPos;

    public string sceneToLoad;
    public string lastScene;

    // Start is called before the first frame update
    void Awake()
    {
       /* if(manager == null)
        {
            manager = this;
          //  SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        }
        else if(manager !=this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        if (!GameObject.Find("PlayerWorldMap"))
        {
            GameObject Player = Instantiate(player, Vector3.zero, Quaternion.identity);
            Player.name = "PlayerWorldMap";
        }
        */
    }



    
}

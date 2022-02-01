using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    public int loadScene;
    public int unloadScene;


    bool loaded;

    
    void OnTriggerEnter()
    {
        if (!loaded)
        {
            Manager.manager.UnloadScene(unloadScene);
            SceneManager.LoadSceneAsync(loadScene, LoadSceneMode.Additive);
            
            loaded = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Manager.manager.UnloadScene(unloadScene);
            SceneManager.LoadSceneAsync(loadScene, LoadSceneMode.Additive);
        }
    }
}

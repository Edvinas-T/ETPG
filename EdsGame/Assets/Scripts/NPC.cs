using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    public int scene;

    bool loaded;

    private void OnTriggerEnter()
    {
        if (!loaded)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

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
        
    }
}

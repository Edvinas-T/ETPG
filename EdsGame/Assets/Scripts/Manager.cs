using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager manager;
    public GameObject player;
    bool gameStart;

    // Start is called before the first frame update
    void Awake()
    {
        if(!gameStart)
        {
            manager = this;
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);


            gameStart = true;
        }
    }

    private void Update()
    {
        
    }
    public void UnloadScene(int scene)
    {
        StartCoroutine(Unload(scene));
    }

    IEnumerator Unload(int scene)
    {
        yield return null;

        SceneManager.UnloadScene(scene);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    public int sceneNo;

    void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(sceneNo);    
    }
}

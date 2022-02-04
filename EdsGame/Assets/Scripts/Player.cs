using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public LayerMask whatCanBeClicked;
    
    public NavMeshAgent player;
    public Animator playerAnimator;

    private void OnTriggerEnter()
    {
        player.isStopped = true;
        transform.position.Set(2, 2, 2);
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        move_forward();
        animate();
        
       
    }
   
    private void move_forward()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, whatCanBeClicked))
            {
                player.isStopped = false;
                player.SetDestination(hit.point);
            }
           
        }
    }

    private void animate()
    {
        if (player.velocity != Vector3.zero)
        {
            playerAnimator.SetBool("isWalking", true);

        }
        else if (player.velocity == Vector3.zero)
        {
            playerAnimator.SetBool("isWalking", false);

        }
    }

  
}

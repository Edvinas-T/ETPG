using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float current_speed;
    private float walking_speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        current_speed = walking_speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (should_move_forward()) move_forward();
    }

    private void move_forward()
    {   
        transform.position += current_speed * transform.forward * Time.deltaTime;
    }

    private bool should_move_forward()
    {
        return Input.GetKey(KeyCode.W);
    }
}

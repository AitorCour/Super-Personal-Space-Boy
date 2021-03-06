﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 axis;
    private Vector2 axis2;
    private Vector2 axis3;
    private PlayerBehaviour player;
    private UI_Manager ui;
    private GameManager gameManager;
    private DynamicJoystick joystick;
    public bool mobileCtrl;
    public bool xboxController;

    public void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI_Manager>();
        joystick = GameObject.FindGameObjectWithTag("DynamicJoystick").GetComponent<DynamicJoystick>();
        gameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 axis = Vector3.zero;
        Vector2 axis2 = Vector2.zero;
        Vector2 axis3 = Vector2.zero;
        if (!mobileCtrl && !xboxController)
        {
            axis2.x = Input.GetAxis("Horizontal");
            //axis.z = Input.GetAxis("Vertical");
            axis2.y = Input.GetAxis("Vertical");
        }
        if(xboxController)
        {
            axis3.x = Input.GetAxisRaw("J_Horizontal");
            axis3.y = Input.GetAxisRaw("J_Vertical");
        }
        else
        {
            axis.x = joystick.Direction.x;
            axis.z = joystick.Direction.y;
        }
        player.Move(axis2);
        player.Move(axis3);

        if (Input.GetButtonDown("Jump") && !ui.cooling)
        {
            player.Attack();
            ui.StartCooldown();
            //cooldown
        }
        if(Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("J_Reboot"))
        {
            gameManager.Restart();
        }
        if(player.attacking)
        {
            player.Attack();
        }

        if(ui.cooling)
        {
            ui.CooldownUpdate();
        }
    }
    public void AttackButton()
    {
        if(!ui.cooling)
        {
            player.Attack();
            ui.StartCooldown();
        }
    }
}

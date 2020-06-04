using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 axis;
    private PlayerBehaviour player;
    private UI_Manager ui;
    private DynamicJoystick joystick;
    public bool mobileCtrl;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI_Manager>();
        joystick = GameObject.FindGameObjectWithTag("DynamicJoystick").GetComponent<DynamicJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 axis = Vector3.zero;
        if(!mobileCtrl)
        {
            axis.x = Input.GetAxis("Horizontal");
            axis.z = Input.GetAxis("Vertical");
        }
        else
        {
            axis.x = joystick.Direction.x;
            axis.z = joystick.Direction.y;
        }
        player.Move(axis);

        if(Input.GetButtonDown("Jump") && !ui.cooling)
        {
            player.Attack();
            ui.StartCooldown();
            //cooldown
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

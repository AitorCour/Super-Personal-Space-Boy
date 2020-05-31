using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 axis;
    private PlayerBehaviour player;
    private UI_Manager ui;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 axis = Vector3.zero;
        axis.x = Input.GetAxis("Horizontal");
        axis.z = Input.GetAxis("Vertical");
        player.Move(axis);

        if(Input.GetButton("Jump") && !ui.cooling || player.attacking)
        {
            player.Attack();
            ui.StartCooldown();
            //cooldown
        }
        if(ui.cooling)
        {
            ui.CooldownUpdate();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RB_Object : MonoBehaviour
{
    private PlayerBehaviour player;
    private UI_Manager ui;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI_Manager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Enemy" && collision.relativeVelocity.magnitude > 4)
        {
            EnemyBehaviour enemy = collision.gameObject.GetComponent<EnemyBehaviour>();
            enemy.RecieveHit();
            Debug.Log("EnemyHitted");
            //EndCooldown
            ui.EndCooldown();
            //player.UpdateHits(1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RB_Object : MonoBehaviour
{
    //private PlayerBehaviour player;
    private UI_Manager ui;

    private Renderer rend;
    private bool parrileable; //si es parrileable, será rosa, y si golpea a un enemigo, le hará daño, ademas de recuperarte la carga
    public Material material_1;
    public Material material_2;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI_Manager>();

        rend = gameObject.GetComponent<Renderer>();
        int rand = Random.Range(0, 2);
        if (rand == 1)
        {
            //Cambio Color
            rend.material = material_1;
            parrileable = true;
        }
        else
        {
            rend.material = material_2;
            parrileable = false;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Enemy" && collision.relativeVelocity.magnitude > 4 && parrileable)
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

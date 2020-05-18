using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RB_Object : MonoBehaviour
{
    private PlayerBehaviour player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Enemy" && collision.relativeVelocity.magnitude > 4)
        {
            EnemyBehaviour enemy = collision.gameObject.GetComponent<EnemyBehaviour>();
            enemy.RecieveHit();
            Debug.Log("EnemyHitted");
            player.UpdateHits(1);
        }
    }
}

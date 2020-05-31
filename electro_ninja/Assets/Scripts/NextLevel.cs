using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public int nextPoint;
    public List<GameObject> origins;
    private PlayerBehaviour player;
    public List<GameObject> enemies;
    private List<bool> deads;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        origins = new List<GameObject>();
        enemies = new List<GameObject>();
        AddTransforms();
        AddEnemies();
    }
    private void AddEnemies()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject target in enemy)
        {
            enemies.Add(target);
            deads.Add(target);
            //deads[1] = false;//hacer un for
        }
    }
    private void CheckDead()
    {
        /*foreach(GameObject enemy in enemies)
        {
            EnemyBehaviour target = enemy.GetComponent<EnemyBehaviour>();
            if(target.dead)
            {
                enemies.Remove(enemy);
                Debug.Log("EnemyDead");
            }
        }*/
        for(int i = 0; i < enemies.Count; i++)
        {
            EnemyBehaviour target = enemies[i].GetComponent<EnemyBehaviour>();
            if (target.dead)
            {
                //enemies.Remove(enemies[i]);
                Debug.Log("EnemyDead");
                deads[i] = true;
            }
        }
    }
    private void AddTransforms()
    {
        foreach(GameObject originObj in GameObject.FindGameObjectsWithTag("Origin"))
        {
            origins.Add(originObj);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        CheckDead();
        //transport
        if (other.tag == "Player")
        {
            //Transport();
        }
        if(enemies == null)
        {
            Debug.Log("AllDead");
        }
    }
    private void Transport()
    {
        player.UpdateHits(1);
        player.transform.position = origins[nextPoint].transform.position;
    }
}

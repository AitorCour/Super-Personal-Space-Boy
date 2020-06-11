using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public int nextPoint;
    public List<GameObject> origins;
    private PlayerBehaviour player;
    public List<GameObject> enemies;

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
        //CheckDead();
        //transport
        if (other.tag == "Player")
        {
            //Transport();
            if (AreAllDead() == true)
            {
                Debug.Log("NextLevelWO");
            }
            else if (AreAllDead() == false)
            {
                Debug.Log("NoPass");
            }
        }
    }
    private void Transport()
    {
        //player.UpdateHits(1);
        player.transform.position = origins[nextPoint].transform.position;
    }
    private bool AreAllDead()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            EnemyBehaviour target = enemies[i].GetComponent<EnemyBehaviour>();
            if (!target.dead)
            {
                return false;
            }
        }
        return true;
    }
}

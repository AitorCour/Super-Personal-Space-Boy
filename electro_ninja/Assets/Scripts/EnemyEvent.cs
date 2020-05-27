using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvent : MonoBehaviour
{
    private EnemyBehaviour enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyBehaviour>();
    }
    private void EndAnimation()
    {
        enemy.attacking = false;
    }
    
}

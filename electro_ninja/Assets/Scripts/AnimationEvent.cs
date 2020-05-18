using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private PlayerBehaviour player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerBehaviour>();
    }
    void EndAnimation()
    {
        player.attacking = false;
        player.UpdateHits(-1);//Hace un ataque, así que lo pierde;
    }
}

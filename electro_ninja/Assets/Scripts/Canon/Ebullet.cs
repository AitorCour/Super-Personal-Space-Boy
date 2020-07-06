﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ebullet : MonoBehaviour
{
    private Rigidbody rb;
    private Transform canon;
    private float iniSpeed;
    public float speed = 200;
    protected Vector3 iniPos;
    public Vector3 dir;
    protected bool shot;
    public bool rebooted;
    private bool canDoDamage;
    public AudioSource shotFX;
    // Use this for initialization
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        iniPos = transform.position;
        rebooted = false;
        canDoDamage = true;
        iniSpeed = speed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(shot)
        {
            //transform.Translate(dir * speed * Time.deltaTime);
            rb.AddForce(canon.up * speed);
        }
        else canDoDamage = false;
    }

    public virtual void ShotBullet(Vector3 origin, Vector3 direction, Transform myCanon)
    {
        shot = true;
        transform.position = origin;
        dir = direction;
        canon = myCanon;
        if(shotFX != null)
        {
            shotFX.volume = Random.Range(0.75f, 0.9f);
            shotFX.pitch = Random.Range(0.9f, 1.1f);
            shotFX.Play();
        }
    }

    public virtual void Reset()
    {
        transform.position = iniPos;
        shot = false;
        canDoDamage = true;
        rebooted = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        speed = iniSpeed;
        //rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    protected void OnTriggerExit(Collider collision)
    {
        if(collision.tag == "Boundary")
        {
            Reset();
            Debug.Log("Boundary");
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (!canDoDamage) return;
            collision.gameObject.GetComponent<PlayerBehaviour>().LoseLife();
        }
        if(collision.gameObject.tag == "Enemy")
        {
            if (!canDoDamage) return;
            collision.gameObject.GetComponent<EnemyBehaviour>().RecieveHit();
        }
        shot = false;
        rb.constraints = RigidbodyConstraints.None;
    }
}
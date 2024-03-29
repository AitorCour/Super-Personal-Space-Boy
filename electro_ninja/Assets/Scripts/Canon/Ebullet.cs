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
    public Vector3 originalEnemy;
    protected bool shot;
    public bool rebooted;
    private bool canDoDamage;
    public AudioSource shotFX;
    // Use this for initialization
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        iniPos = transform.position;
        rb.useGravity = false;
        rebooted = false;
        canDoDamage = true;
        iniSpeed = speed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (shot)
        {
            //transform.Translate(dir * speed * Time.deltaTime);
            //rb.AddForce(canon.up * speed);
            transform.Translate(dir.normalized * speed * Time.deltaTime);
            //rb.velocity = dir * speed* 100 * Time.deltaTime;
            rb.useGravity = false;
            Debug.Log("Shoot_2");
        }
        else
        {
            canDoDamage = false;
        }
    }
    //originalEnemy - 
    public virtual void ShotBullet(Vector3 origin, Vector3 direction, Transform myCanon)
    {
        shot = true;
        originalEnemy = origin;
        transform.position = origin;
        dir = direction;
        canon = myCanon;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        if (shotFX != null)
        {
            shotFX.volume = Random.Range(0.75f, 0.9f);
            shotFX.pitch = Random.Range(0.9f, 1.1f);
            shotFX.Play();
        }
    }
    public virtual void ShotBulletToEnemy(Vector3 origin, Vector3 direction)
    {
        shot = true;
        dir = direction;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        speed *= 2;
        if (shotFX != null)
        {
            shotFX.volume = Random.Range(0.75f, 0.9f);
            shotFX.pitch = Random.Range(0.9f, 1.1f);
            shotFX.Play();
        }
        Debug.Log("Shoot_3");
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
        if(collision.gameObject.tag == "Player" && !rebooted)
        {
            if (!canDoDamage) return;
            collision.gameObject.GetComponent<PlayerBehaviour>().LoseLife();
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
        if(collision.gameObject.tag == "Enemy")
        {
            if (!canDoDamage) return;
            collision.gameObject.GetComponent<EnemyBehaviour>().RecieveHit();
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Untagged")
        {
            canDoDamage = false;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
        shot = false;
        //rb.constraints = RigidbodyConstraints.None;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ebullet : MonoBehaviour
{
    
    public float speed;

    protected bool shot;
    protected Vector3 iniPos;
    protected Vector3 dir;

    public AudioSource shotFX;
    // Use this for initialization
    protected virtual void Start()
    {
        iniPos = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(shot)
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }
    }

    public virtual void ShotBullet(Vector3 origin, Vector3 direction)
    {
        shot = true;
        transform.position = origin;
        dir = direction;

        if(shotFX != null)
        {
            shotFX.volume = Random.Range(0.75f, 0.9f);
            shotFX.pitch = Random.Range(0.9f, 1.1f);
            shotFX.Play();
        }
    }

    /*public virtual void ShotBullet(Vector3 origin, float zRot)
    {
        ShotBullet(origin, Vector2.down);
        transform.rotation = Quaternion.Euler(0, 0, zRot);
    }*/

    public virtual void Reset()
    {
        transform.position = iniPos;
        shot = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
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
            collision.gameObject.GetComponent<PlayerBehaviour>().LoseLife();
            Debug.Log("PlayerGet");
            //collision.gameObject.SendMessage("Damage", damage);

            Reset();
        }
    }
}

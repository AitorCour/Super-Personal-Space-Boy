using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ecanon : MonoBehaviour
{
    
    public int maxAmmo;
    public GameObject bulletPrefab;
    public Transform ammoTransform;
    public Transform gunTransform;
    public Ebullet[] bullets;
    //private float timeCounter;
    //private bool canShot = false;

    private int currentBullet = 0;
    //public float cooldown;

    //public float timeShot;

    void Start ()
    {
        CreateBullets();
    }
    void CreateBullets()
    {
        bullets = new Ebullet[maxAmmo];

        for(int i = 0; i < maxAmmo; i++)
        {
            Vector3 spawnPos = ammoTransform.position;
            spawnPos.x -= i * 0.5f;//se instancian o.5 mas
            GameObject b = Instantiate(bulletPrefab, spawnPos, Quaternion.identity, ammoTransform);
            b.name = "Bullet_" + i;
            bullets[i] = b.GetComponent<Ebullet>();
        }

        //canShot = true;
        //timeCounter = 0;
    }

    public void ShotBullet(Vector3 player)
    {
        bullets[currentBullet].ShotBullet(gunTransform.position, player - gunTransform.position);
        currentBullet++;
        if(currentBullet >= maxAmmo) currentBullet = 0;
    }
    /*public void ShotRotateBullets()
    {
        float zRot = transform.eulerAngles.z;

        bullets[currentBullet].ShotBullet(transform.position, zRot - 90);
        currentBullet++;
        if (currentBullet >= maxAmmo) currentBullet = 0;
    }*/
}

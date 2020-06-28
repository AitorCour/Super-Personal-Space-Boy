using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : EnemyBehaviour
{
    public GameObject objToRotate;
    private Ecanon canon;
    private float attackTime = 2;
    private bool waiting;

    // Start is called before the first frame update
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI_Manager>();
        animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        canon = GetComponent<Ecanon>();
        dead = false;
        detected = false;
        life = 1;

        colliders = new List<BoxCollider>();
        AddColliders(transform);
        foreach (BoxCollider bc in colliders)
        {
            bc.enabled = false;
        }

        rigidBody = new List<Rigidbody>();
        AddRigidbodys(transform);
        foreach (Rigidbody rb in rigidBody)
        {
            rb.useGravity = false;
        }

        transformChilds = new List<Transform>();
        AddTransforms(transform);

        renderers = new List<MeshRenderer>();
        AddRenderers(transform);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
        //Rotate TowardsPlayer
        Vector3 targetDirection = player.transform.position - objToRotate.transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(objToRotate.transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(objToRotate.transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        objToRotate.transform.rotation = Quaternion.LookRotation(newDirection);

        if(distance < attackDistance && !attacking)
        {
            Attack();
        }
        else if (distance < attackDistance && attacking && !waiting)
        {
            StartCoroutine(WaitAttack());
            waiting = true;
        }
    }

    private IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(attackTime);
        attacking = false;
        waiting = false;
    }
    private void Attack()
    {
        Debug.Log("Shoot");
        attacking = true;
        canon.ShotBullet(player.transform.position);
    }
}

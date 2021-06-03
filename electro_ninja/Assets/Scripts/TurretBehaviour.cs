using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : EnemyBehaviour
{
    public GameObject objToRotate;
    private Ecanon canon;
    public float attackTime = 2;
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
        if (dead) return;
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

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, radius, mask))//Set detected
        {
            if (hit.collider != null && hit.collider.tag == "Player")
            {
                detected = true;
            }
            else detected = false;
        }

        if (distance < radius && !attacking && detected)
        {
            Attack();
        }
        else if (distance < radius && attacking && !waiting && detected)
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
    protected override void Dead()
    {
        capsuleCollider.enabled = false;
        canon.dead = true;
        canon.enabled = false;
        //animator.enabled = false;
        dead = true;
        ui.UpdateScore(200);

        //SeparateChildrens();
        foreach (BoxCollider bc in colliders)
        {
            bc.enabled = true;
        }
        foreach (Rigidbody rb in rigidBody)
        {
            rb.useGravity = true;
        }
        foreach (Transform tr in transformChilds)
        {
            tr.parent = null;
        }
        StartCoroutine(FallRest());
        //Reconstruct();
    }
}

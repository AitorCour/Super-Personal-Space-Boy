using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected PlayerBehaviour player;
    protected UI_Manager ui;
    protected Animator animator;
    protected CapsuleCollider capsuleCollider;
    public LayerMask mask;

    public float speed;
    public float attackDistance;
    public float radius;
    protected float distance;
    protected float life;

    public float frontFOV;

    public bool detected;
    public bool sideLD;
    private bool sideRD;
    public bool dead;
    public bool attacking;
    private bool optimized;

    public List<BoxCollider> colliders;
    public List<Rigidbody> rigidBody;
    public List<Transform> transformChilds;
    public List<MeshRenderer> renderers;

    //test
    public Transform rotateObj;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI_Manager>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
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

        agent.speed = speed;
        agent.enabled = true;
    }
    protected virtual void AddColliders(Transform t)
    {
        for(int i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            AddColliders(child);
            BoxCollider c = child.gameObject.GetComponent<BoxCollider>();
            if(c != null)
            {
                colliders.Add(c);
            }
        }
    }
    protected virtual void AddRigidbodys(Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            AddRigidbodys(child);
            Rigidbody r = child.gameObject.GetComponent<Rigidbody>();
            if (r != null)
            {
                rigidBody.Add(r);
            }
        }
    }
    protected virtual void AddTransforms(Transform t)
    {
        for(int i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            AddTransforms(child);
            Transform tr = child.gameObject.GetComponent<Transform>();
            if (tr != null)
            {
                transformChilds.Add(tr);
            }
        }
    }
    protected virtual void AddRenderers(Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            AddRenderers(child);
            MeshRenderer mr = child.gameObject.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                renderers.Add(mr);
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.red;
        if (player != null)
        {
            Gizmos.DrawLine(transform.position, player.transform.position);
            float r = frontFOV * Mathf.Deg2Rad;
            Vector3 right = (rotateObj.transform.forward * Mathf.Cos(r) + rotateObj.transform.right * Mathf.Sin(r)).normalized;
            Gizmos.DrawRay(rotateObj.transform.position, right * radius/4);

            Gizmos.color = Color.green;
            float l = -frontFOV * Mathf.Deg2Rad;
            Vector3 left = (rotateObj.transform.forward * Mathf.Cos(l) + rotateObj.transform.right * Mathf.Sin(l)).normalized;
            Gizmos.DrawRay(rotateObj.transform.position, left * radius/4);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Rotate TowardsPlayer
        Vector3 targetDirection = player.transform.position - rotateObj.transform.position;
        // The step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(rotateObj.transform.forward, targetDirection, singleStep, 0.0f);
        rotateObj.transform.rotation = Quaternion.LookRotation(newDirection);

        if (dead) return;
        distance = Vector3.Distance(player.transform.position, transform.position);

        float l = -frontFOV * Mathf.Deg2Rad;
        Vector3 leftRayDir = (rotateObj.transform.forward * Mathf.Cos(l) + rotateObj.transform.right * Mathf.Sin(l)).normalized;
        float r = frontFOV * Mathf.Deg2Rad;
        Vector3 rightRayDir = (rotateObj.transform.forward * Mathf.Cos(r) + rotateObj.transform.right * Mathf.Sin(r)).normalized;

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, (player.transform.position-transform.position), out hit, radius, mask))//Set detected
        {
            if (hit.collider != null && hit.collider.tag == "Player" && sideLD && sideRD)
            {
                detected = true;
                if (optimized) Deoptimize();
            }
            else detected = false;
        }
        if (Physics.Raycast(rotateObj.transform.position, rightRayDir, out hit, radius / 4, mask))
        {
            if (hit.collider != null && hit.collider.tag != "Player")
            {
                //Debug.Log("Ground");
                sideRD = false;
                Debug.Log(hit.collider.name);
            }
            else sideRD = true;
        }
        else sideRD = true;
        if (Physics.Raycast(rotateObj.transform.position, leftRayDir, out hit, radius / 4, mask))
        {
            if (hit.collider != null && hit.collider.tag != "Player")
            {
                //Debug.Log("Ground");
                sideLD = false;
                Debug.Log(hit.collider.name);
            }
            else sideLD = true;
        }
        sideLD = true;

        if (distance <= attackDistance)//Start Attack
        {
            agent.isStopped = true;
            if(!attacking)
            {
                Attack();
            }
            //DODAMAGE
        }
        else if (distance > attackDistance && detected && !attacking)//Go to player
        {
            animator.SetBool("Walking", true);
            animator.SetBool("Attacking", false);
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            attacking = false;
        }
        if(!detected)//Idle
        {
            animator.SetBool("Walking", false);
            agent.isStopped = true;
            
            attacking = false;

            if (distance >= radius * 2 && !optimized)
            {
                //Debug.Log("Optimo");
                Optimize();
            }
            else if (distance < radius * 2)
            {
                Deoptimize();
            }
        }
        /*else if(distance <= radius)//Set detected
        {
            detected = true;
            if (optimized) Deoptimize();
        }*/
    }
    private void Optimize()
    {
        animator.enabled = false;
        foreach(MeshRenderer mr in renderers)
        {
            mr.enabled = false;
        }
        optimized = true;
    }
    private void Deoptimize()
    {
        optimized = false;
        animator.enabled = true;
        foreach (MeshRenderer mr in renderers)
        {
            mr.enabled = true;
        }
    }
    public void RecieveHit()
    {
        life = -1;
        if(life <= 0)
        {
            Dead();
        }
    }
    protected virtual void Dead()
    {
        capsuleCollider.enabled = false;
        agent.enabled = false;
        animator.enabled = false;
        dead = true;
        ui.UpdateScore(100);

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
    private void Attack()
    {
        animator.SetBool("Attacking", true);
        animator.SetBool("Walking", false);
        attacking = true;
    }
    protected virtual IEnumerator FallRest()
    {
        yield return new WaitForSeconds(10);
        foreach (BoxCollider bc in colliders)
        {
            bc.isTrigger = true;
        }
        foreach (Rigidbody rb in rigidBody)
        {
            rb.useGravity = false;
        }
        StartCoroutine(Reactive());
        //https://answers.unity.com/questions/1230671/how-to-fade-out-a-game-object-using-c.html
        //HACER UN FADE
    }
    private IEnumerator Reactive()
    {
        yield return new WaitForSeconds(2);
        foreach (Rigidbody rb in rigidBody)
        {
            rb.useGravity = true;
        }
    }
    //Cada eelemento separedo del enemigo tiene un rigidbody, y en el momento de golpear, se "activan" estos rigidbody, el navmesh se desactiva, y se desemparentan los componentes
}

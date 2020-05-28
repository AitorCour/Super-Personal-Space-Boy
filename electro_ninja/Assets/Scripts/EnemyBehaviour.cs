using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected PlayerBehaviour player;
    protected Animator animator;
    private CapsuleCollider capsuleCollider;
    public float speed;
    public float attackDistance;
    public float radius;
    public float distance;
    private float life;
    public bool detected;
    protected bool dead;
    public bool attacking;
    private bool moving;

    public List<BoxCollider> colliders;
    public List<Rigidbody> rigidBody;
    public List<Transform> transformChilds;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        dead = false;
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

        agent.speed = speed;
        agent.enabled = true;
    }
    private void AddColliders(Transform t)
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
    private void AddRigidbodys(Transform t)
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
    private void AddTransforms(Transform t)
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
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    // Update is called once per frame
    void Update()
    {
        if (dead) return;

        distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= attackDistance)
        {
            agent.isStopped = true;
            moving = false;
            if(!attacking)
            {
                Attack();
            }
            //DODAMAGE
        }
        else if (distance > attackDistance && detected)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            moving = true;
            attacking = false;
        }
        if(distance > radius)
        {
            agent.isStopped = true;
            detected = false;

            moving = false;
            attacking = false;
        }
        else if(distance <= radius)
        {
            detected = true;
        }
        if(moving)
        {
            animator.SetBool("Walking", true);
        }
        else if(!moving && !attacking)
        {
            animator.SetBool("Walking", false);
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
    private void Dead()
    {
        capsuleCollider.enabled = false;
        agent.enabled = false;
        animator.enabled = false;
        dead = true;
        //SeparateChildrens();
        foreach (BoxCollider bc in colliders)
        {
            bc.enabled = true;
        }
        foreach (Rigidbody rb in rigidBody)
        {
            rb.useGravity = true;
        }
        StartCoroutine(Minimize());
        //Reconstruct();
    }
    private void Attack()
    {
        animator.SetBool("Attacking", true);
        animator.SetBool("Walking", false);
        attacking = true;
    }
    private IEnumerator Minimize()
    {
        yield return new WaitForSeconds(20);
        foreach (Transform tr in transformChilds)
        {
            tr.localScale = new Vector3(0, 0, 0);

        }
        //https://answers.unity.com/questions/1230671/how-to-fade-out-a-game-object-using-c.html
        //HACER UN FADE
    }

    /*private void Reconstruct()
    {
        Debug.Log("reconstruction");
        //Los aparta del mapa
        foreach (BoxCollider bc in colliders)
        {
            bc.enabled = false;
        }
        foreach (Rigidbody rb in rigidBody)
        {
            rb.useGravity = false;
        }
        //ResetPosition();


    }*/
    /*private void ResetPosition()
    {
        Debug.Log("reseting");
        for (int i = 0; i < transform.childCount; i++)
        {
            //transformChilds[i].position = vectorChilds[i];
            transformChilds[i].position = Vector3.zero;
            transformChilds[i].rotation = Quaternion.Euler(0, 0, 0);
        }
    }*/
    //Cada eelemento separedo del enemigo tiene un rigidbody, y en el momento de golpear, se "activan" estos rigidbody, el navmesh se desactiva, y se desemparentan los componentes
}

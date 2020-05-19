using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected PlayerBehaviour player;
    private Animator animator;
    private CapsuleCollider capsuleCollider;
    public float speed;
    public float attackDistance;
    public float radius;
    public float distance;
    public bool detected;
    public List<BoxCollider> colliders;
    public List<Rigidbody> rigidBody;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        colliders = new List<BoxCollider>();
        AddColliders(transform);
        foreach(BoxCollider bc in colliders)
        {
            bc.enabled = false;
        }

        rigidBody = new List<Rigidbody>();
        AddRigidbodys(transform);
        foreach (Rigidbody rb in rigidBody)
        {
            rb.useGravity = false;
        }

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
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    // Update is called once per frame
    void Update()
    {
        if (!agent.enabled) return;

        distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= attackDistance)
        {
            agent.isStopped = true;
            Attack();
        }
        else if (distance > attackDistance && detected)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        if(distance > radius)
        {
            agent.isStopped = true;
            detected = false;
        }
        else if(distance <= radius)
        {
            detected = true;
        }
    }
    public void RecieveHit()
    {
        capsuleCollider.enabled = false;
        agent.enabled = false;
        animator.enabled = false;

        foreach (BoxCollider bc in colliders)
        {
            bc.enabled = true;
        }
        foreach (Rigidbody rb in rigidBody)
        {
            rb.useGravity = true;
        }
    }
    private void Attack()
    {
        //Ejecutar animación con tiempo de salida
    }
    //Cada eelemento separedo del enemigo tiene un rigidbody, y en el momento de golpear, se "activan" estos rigidbody, el navmesh se desactiva, y se desemparentan los componentes
}

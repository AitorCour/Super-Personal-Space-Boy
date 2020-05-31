using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject model;
    private Animator animator;
    private RaycastCircle attack;
    private UI_Manager ui;
    private CapsuleCollider capsuleCollider;
    private Rigidbody myRigidbody;
    public float speed = 10f;
    public float rayDistance;
    public float rayDistance2;
    public bool attacking;
    public bool canWalk;
    private bool dead;
    private int life = 1;
    public int attackNum = 1;

    private Vector3 moveDirection;
    public LayerMask mask;

    public List<BoxCollider> colliders;
    public List<Rigidbody> rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        attack = GetComponent<RaycastCircle>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigidbody = GetComponent<Rigidbody>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI_Manager>();
        ui.Initialize();
        ui.UpdateHitCounter(attackNum);
        dead = false;


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
    }
    private void AddColliders(Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            AddColliders(child);
            BoxCollider c = child.gameObject.GetComponent<BoxCollider>();
            if (c != null)
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
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.blue;
        Vector3 direction = model.transform.TransformDirection(Vector3.back) * rayDistance;
        Gizmos.DrawRay(model.transform.position, direction); //forward

        Vector3 directionG = model.transform.TransformDirection(Vector3.down) * rayDistance2;
        Gizmos.DrawRay(model.transform.position, directionG); //forward
        Gizmos.DrawRay(model.transform.position + new Vector3(0,0,0.4f), directionG); //forward
    }
    void Update()
    {
        if (dead) return;
        Vector3 direction = model.transform.TransformDirection(Vector3.back);
        Vector3 directionG = model.transform.TransformDirection(Vector3.down);

        if (Physics.Raycast(model.transform.position, direction, rayDistance, mask))
        {
            canWalk = false;
        }
        else if (!Physics.Raycast(model.transform.position, direction, rayDistance, mask))
        {
            canWalk = true;
        }

        if (Physics.Raycast(model.transform.position, directionG, rayDistance2, mask) || 
            Physics.Raycast(model.transform.position + new Vector3(0, 0, -0.4f), directionG, rayDistance2, mask) ||
            Physics.Raycast(model.transform.position + new Vector3(0, 0, 0.4f), directionG, rayDistance2, mask) ||
            Physics.Raycast(model.transform.position + new Vector3(-0.4f, 0, 0), directionG, rayDistance2, mask) ||
            Physics.Raycast(model.transform.position + new Vector3(0.4f, 0, 0), directionG, rayDistance2, mask))
        {
            canWalk = true;
            myRigidbody.drag = 1;
        }
        else if (!Physics.Raycast(model.transform.position, directionG, rayDistance2, mask) || !Physics.Raycast(model.transform.position + new Vector3(0, 0, -0.4f), directionG, rayDistance2, mask) || !Physics.Raycast(model.transform.position + new Vector3(0, 0, 0.4f), directionG, rayDistance2, mask) || !Physics.Raycast(model.transform.position + new Vector3(-0.4f, 0, 0), directionG, rayDistance2, mask) || !Physics.Raycast(model.transform.position + new Vector3(0.4f, 0, 0), directionG, rayDistance2, mask))
        {
            canWalk = false;
            myRigidbody.drag = 0;
        }
    }
    public void Move(Vector3 direction)
    {
        if (dead) return;
        //character.Move(new Vector3(direction.x, 0, direction.z) * speed * Time.deltaTime);
        if(canWalk)
        {
            transform.Translate(new Vector3(direction.x, 0, direction.z) * speed * Time.deltaTime);
            
        }
        else
        {
            animator.SetBool("Walking", false);
        }
        //Debug.Log(direction.x);

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction * -1);
            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, lookRotation, 5);
            if (canWalk)
            {
                animator.SetBool("Walking", true);
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }
    public void Attack()//El attack solo se manda mientras se pulsa el boton
    {
        if (/*attackNum <= 0 ||*/ dead) return;

        else if(attackNum < 0)
        {
            attackNum = 0;
        }
        if(!attacking)
        {
            animator.SetTrigger("Attack");
            attacking = true;
            //cube.AddForce(transform.forward * 10, ForceMode.Impulse);
            //attack.Attack();
        }
        else if(attacking)//hacer que esto se llame incluso cuando no se pulsa
        {
            attack.Attack();
        }
    }
    public void UpdateHits(int hit)
    {
        attackNum += hit;
        ui.UpdateHitCounter(attackNum);
    }
    public void LoseLife()
    {
        life -= 1;
        Debug.Log("Dead");
        if(life >= 0)
        {
            Dead();
        }
    }
    private void Dead()
    {
        capsuleCollider.enabled = false;
        myRigidbody.useGravity = false;
        //agent.enabled = false;
        animator.enabled = false;
        attacking = false;
        dead = true;
        foreach (BoxCollider bc in colliders)
        {
            bc.enabled = true;
        }
        foreach (Rigidbody rb in rigidBody)
        {
            rb.useGravity = true;
        }
    }
}

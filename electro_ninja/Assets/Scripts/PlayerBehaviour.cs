using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private CharacterController character;
    public GameObject model;
    private Animator animator;
    private RaycastCircle attack;
    private UI_Manager ui;
    public float speed = 10f;
    public float rayDistance;
    public bool attacking;
    private bool canWalk;
    private int life;
    public int attackNum = 1;

    private Vector3 moveDirection;
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        attack = GetComponent<RaycastCircle>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI_Manager>();
        ui.Initialize();
        ui.UpdateHitCounter(attackNum);
    }
    void OnDrawGizmosSelected()
    {
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.blue;
        Vector3 direction = model.transform.TransformDirection(Vector3.back) * rayDistance;
        Gizmos.DrawRay(model.transform.position, direction); //forward
    }
    void Update()
    {
        Vector3 direction = model.transform.TransformDirection(Vector3.back);

        if (Physics.Raycast(model.transform.position, direction, rayDistance, mask))
        {
            canWalk = false;
        }
        else if (!Physics.Raycast(model.transform.position, direction, rayDistance, mask))
        {
            canWalk = true;
        }
    }
    public void Move(Vector3 direction)
    {
        //character.Move(new Vector3(direction.x, 0, direction.z) * speed * Time.deltaTime);
        if(canWalk)
        {
            transform.Translate(new Vector3(direction.x, 0, direction.z) * speed * Time.deltaTime);
        }
        //Debug.Log(direction.x);

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction * -1);
            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, lookRotation, 5);
        }
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("OOF");
        }
    }*/
    public void Attack()//El attack solo se manda mientras se pulsa el boton
    {
        if (attackNum <= 0) return;
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_Enemy : EnemyBehaviour
{
    public LayerMask mask;
    public float acceleration;
    public float speedRot;
    private Vector3 destination;
    public bool destinationAchieved;
    private bool waiting;
    private bool endWait;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        agent.acceleration = acceleration;
        destinationAchieved = false;
        waiting = false;
        endWait = false;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * radius;
        Gizmos.DrawRay(transform.position, direction); //forward
    }
    // Update is called once per frame
    void Update()
    {
        if (dead) return;

        RaycastHit hit = new RaycastHit();
        Vector3 direction = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, direction, out hit, radius, mask))
        {
            if (hit.collider != null && hit.collider.tag == "Player" && !detected && !destinationAchieved)
            {
                //Debug.Log("Player detected");
                destination = player.transform.position /*- new Vector3(2, 0, 0)*/;
                detected = true;
            }
        }

        distance = Vector3.Distance(destination, transform.position);
        if (distance > attackDistance && detected) //si lo detecta, va a por él
        {
            agent.isStopped = false;
            agent.SetDestination(destination);
            destinationAchieved = false;
            waiting = false;
            endWait = false;
            animator.SetBool("Running", true);
        }
        else if(distance <= attackDistance && detected) // si la distancia con el destino, es menos a la distancia de ataque, se para y rota
        {//if(player.x > transform.x = +3, else -3
            //Debug.Log("destination achieved");
            destinationAchieved = true;
            detected = false;
            animator.SetBool("Running", false);
            //StartCoroutine(WaitRotate());
            //Poner un counter para la rotación
        }
        else if(!detected && !waiting)
        {
            Debug.Log("StartWait");
            StartCoroutine(WaitRotate());
            waiting = true;
            endWait = false;
        }
        else if(!detected && waiting && endWait)
        {
            Rotate();
        }
    }
    private void Rotate()
    {
        if (dead) return;

        Debug.Log("Rotating");
        agent.isStopped = true;
        float speedRot = Time.deltaTime * speed;
        transform.Rotate(Vector3.up * speedRot, Space.World);
        destinationAchieved = false;
    }
    private IEnumerator WaitRotate()
    {
        yield return new WaitForSeconds(1);
        endWait = true; 
        Debug.Log("EndWait");
    }
}

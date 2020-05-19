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
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        agent.acceleration = acceleration;
        destinationAchieved = true;
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
        RaycastHit hit = new RaycastHit();
        Vector3 direction = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, direction, out hit, radius, mask))
        {
            //Debug.Log(hit.transform.name);
            //Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red, 10.0f);
            if (hit.collider != null && hit.collider.tag == "Player" && !detected && destinationAchieved)
            {
                Debug.Log("Player detected");
                destination = player.transform.position - new Vector3(2, 0, 0);
                detected = true;
            }
            else if (hit.collider.tag != "Player")
            {
                Debug.Log("Player NOT detected");
                //detected = false;
            }
        }

        distance = Vector3.Distance(destination, transform.position);
        if (detected) //si lo detecta, va a por él
        {
            agent.isStopped = false;
            agent.SetDestination(destination);
            destinationAchieved = false;
        }
        else if(!detected) //si no lo detecta, se queda en el sitio rotando
        {
            //agent.SetDestination(destination);
            Rotate();
        }
        if(distance + 3 <= attackDistance && detected) // si la distancia con el destino, es menos a la distancia de ataque, se para y rota
        {//if(player.x > transform.x = +3, else -3
            Debug.Log("destination achieved");
            destinationAchieved = true;
            detected = false;
            //Poner un counter para la rotación
        }
        else if (distance > attackDistance && detected)
        {
            Debug.Log("Chase Player");
        }

        if (agent.isStopped)
        {
            Debug.Log("STOP");
        }
    }
    private void Rotate()
    {
        agent.isStopped = true;

        float speedRot = Time.deltaTime * speed;
        transform.Rotate(Vector3.up * speedRot, Space.World);
    }
}

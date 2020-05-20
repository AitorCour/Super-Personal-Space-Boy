using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCircle : MonoBehaviour
{
    private EnemyBehaviour enemy;
    public float rayDistance;
    public int force;
    private float upForce = 1f;
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rayDistance);
    }
    public void Attack()
    {
        Debug.Log("Attackinggg");
        Vector3 origin = transform.position;
        Collider[] colliders = Physics.OverlapSphere(origin, rayDistance);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            Collider col = hit.GetComponent<Collider>();

            if (col.tag != "Player")
            {
                if (rb != null)
                {
                    rb.AddExplosionForce(force, origin, rayDistance, upForce, ForceMode.Impulse);
                }
            }
            
            if (col != null && col.tag == "Enemy")
            {
                Debug.Log("No null");
                //hit.rigidbody.AddForce(direction * hitForce, ForceMode.Impulse);
                EnemyBehaviour target = hit.transform.gameObject.GetComponent<EnemyBehaviour>();
                target.RecieveHit();
            }
                /*EnemyBehaviour target = hit.transform.gameObject.GetComponent<EnemyBehaviour>();*/
        }
    }
}

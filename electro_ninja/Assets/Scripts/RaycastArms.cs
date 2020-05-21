using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastArms : MonoBehaviour
{
    public Transform mesh;
    public float rayDistance;
    public LayerMask mask;
    public float hitForce;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInChildren<Transform>();
    }
    void OnDrawGizmosSelected()
    {
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.blue;
        Vector3 direction = mesh.transform.TransformDirection(Vector3.forward) * rayDistance;
        Gizmos.DrawRay(mesh.transform.position, direction); //forward
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void ShotTPS()
    {
        RaycastHit hit = new RaycastHit();
        Vector3 direction = mesh.transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(mesh.position, direction, out hit, rayDistance, mask))
        {
            Debug.Log(hit.transform.name);
            //Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red, 10.0f);
            if (hit.rigidbody != null /*&& hit.rigidbody.tag == "Enemy"*/)
            {
                Debug.Log("No null");
                hit.rigidbody.AddForce(direction * hitForce, ForceMode.Impulse);
            }
        }
    }
}

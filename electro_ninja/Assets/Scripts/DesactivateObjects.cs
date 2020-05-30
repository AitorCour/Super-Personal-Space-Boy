using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivateObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Desactivate");
        other.attachedRigidbody.useGravity = false;
        other.enabled = false;
        other.gameObject.SetActive(false);
    }
}

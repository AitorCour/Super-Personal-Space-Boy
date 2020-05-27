using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public int nextPoint;
    public List<GameObject> origins;
    private PlayerBehaviour player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        origins = new List<GameObject>();
        AddTransforms();
    }
    private void AddTransforms()
    {
        foreach(GameObject originObj in GameObject.FindGameObjectsWithTag("Origin"))
        {
            origins.Add(originObj);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //transport
        if(other.tag == "Player")
        {
            Transport();
        }
    }
    private void Transport()
    {
        player.UpdateHits(1);
        player.transform.position = origins[nextPoint].transform.position;
    }
}

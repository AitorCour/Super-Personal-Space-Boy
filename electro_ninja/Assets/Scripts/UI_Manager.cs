using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    private Text hitCounter;
    // Start is called before the first frame update
    public void Initialize()
    {
        hitCounter = GameObject.FindGameObjectWithTag("HitText").GetComponent<Text>();
    }

    public void UpdateHitCounter(int hits)
    {
        string h = "hits: " + hits;
        hitCounter.text = h;
    }
}

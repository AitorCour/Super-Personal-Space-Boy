using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    private Text hitCounter;
    public Image coolImage;

    public float cooldownTime;
    public bool cooling;
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
    public void StartCooldown()
    {
        cooling = true;
    }
    public void CooldownUpdate()
    {
        coolImage.fillAmount += 1 / cooldownTime * Time.deltaTime;
        if (coolImage.fillAmount >= 1)
        {
            cooling = false;
            coolImage.fillAmount = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    private Text hitCounter;
    public Image coolImage;

    public float cooldownTime;
    private int totalScore;
    public bool cooling;
    // Start is called before the first frame update
    public void Initialize()
    {
        hitCounter = GameObject.FindGameObjectWithTag("HitText").GetComponent<Text>();
    }

    public void UpdateScore(int score)
    {
        totalScore = totalScore + score;
        string h = "Score: " + totalScore;
        hitCounter.text = h;
        Debug.Log(totalScore);
    }
    public void StartCooldown()
    {
        cooling = true;
        Debug.Log("StartAttack");
    }
    public void CooldownUpdate()
    {

        coolImage.fillAmount += 1 / cooldownTime * Time.deltaTime;
        if (coolImage.fillAmount >= 1 && cooling)
        {
            EndCooldown();
        }
    }
    public void EndCooldown()
    {
        Debug.Log("EndCooldown");
        cooling = false;
        coolImage.fillAmount = 0;
    }
}

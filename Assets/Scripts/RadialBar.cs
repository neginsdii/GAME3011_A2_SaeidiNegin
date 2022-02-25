using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RadialBar : MonoBehaviour
{
    public Image Fill;
    public float gameTime;
    private float timer;



    void Start()
    {
       
        timer = gameTime;
        Fill.fillAmount = Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if ((!LockState.lsLockBroken || !LockState.isUnlocked) && LockState.PinIsMoved)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
              
                timer = 0;
                LockState.isTimeOver = true;
            }
            else
            {
                Fill.fillAmount = Normalize();
            }
        }
    }

    private float Normalize()
	{
        return (float)timer / gameTime;
	}
}


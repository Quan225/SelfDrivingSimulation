using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayValue : MonoBehaviour {

    public Text hitHistory;
    public Text hitAverage;
    public Text trainingTime;

    float timeTempHistory;
    float timeTempTraining;
    float valHitAverage;

    static DisplayValue instance;

   public bool enableTraining = true;

   

    void Start()
    {
        if (instance == null) instance = this;
    }

    public static DisplayValue Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DisplayValue();
            }
            return instance;
        }
    }


    public void ChangeValue(string changeText)  // 화면에 표시되는 값 들을 갱신해준다.
    {
        float value = 0.0f;
        
        switch (changeText)
        {
            case "hitHistory":
                //if(timeTempHistory >= 0.1f) // 0.1초 안에 충돌이 다시 발생할 경우 오차라고 판단해 기록하지 않는다.
                //{
                //    value = timeTempHistory;
                //    value = Mathf.Round(value * 100.0f) * 0.01f;
                //    hitHistory.text = hitHistory.text + "\nhit : " + value;

                //    if (valHitAverage == 0.0f)  // 처음 기록되는 경우에는
                //    {
                //        valHitAverage = value;  // 그대로 값을 저장한다.
                //    }
                //    else
                //    {
                //        valHitAverage = (valHitAverage + value) * 0.5f;
                //    }
                //    hitAverage.text = "Hit Average : " + Mathf.Round(valHitAverage * 100.0f) * 0.01f; 
                //    // 정확한 값은 아니겠지만. 소수점 2자리에서 끊는다.

                //    timeTempHistory = 0.0f;
                //}
                break;
            case "trainingTime":
                value = timeTempTraining;
                value = Mathf.Round(value * 100.0f) * 0.01f;
                trainingTime.text = "Training Time : " + value;
                break;
            case "hitAverage":
               
                break;
        }
    }


    public void TrainingDisplayOnOff(bool isOn)
    {
        enableTraining = isOn;
    }

    private void Update()
    {
        timeTempHistory += Time.deltaTime;

        if (enableTraining)
        {
            timeTempTraining += Time.deltaTime;
            ChangeValue("trainingTime");

        }

    }
}

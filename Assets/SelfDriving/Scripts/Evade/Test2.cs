using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public CarCtr agent;

    public int historyNum = 1;
    public int rayNum = 2;
    public int actionNum = 2;

    public bool isTraining = true;

    public bool trainingEnable = true;

    public Reinforcement reinforcement;


    VectorArray tempOutputVector;   // Current Frame의 Q값을 저장할 Vector
    VectorArray rewardVec;  // Next Frame Action의 Q값을 저장할 Vector

    float traingTime = 0.0f;
    float hitHistory = 0.0f;



    void Start()
    {
        rewardVec = new VectorArray();
        rewardVec.Initialize(3);
        tempOutputVector = new VectorArray();
        
        reinforcement.num_history = historyNum;
        reinforcement.num_stateVal = rayNum;
        reinforcement.num_action = actionNum;

        reinforcement.Initialize();
        reinforcement.forward(agent.carRay.rayData);
    }

    private void Update()
    {
        reinforcement.MakeInputVector(agent.carRay.rayData);
        reinforcement.nn.setInputVector( reinforcement.oldInputVector);  // input Vector Setting
        reinforcement.nn.PropForward();
        reinforcement.nn.CopyOutputVector( tempOutputVector, false);
        int selectAction = getSelectAction(tempOutputVector);   // Max(Q)를 선별
        agent.carUpdate(selectAction);
    }

    public void Training( VectorArray targetY)
    {
        reinforcement.nn.PropBackward( targetY);
    }

    

    int getSelectAction(VectorArray outputVector)
    {
        int minOutputElement = 0;
        
        double minTemp = outputVector.data[0];
        for (int i = 1; i < reinforcement.num_action; i++)
        {
            if (minTemp > outputVector.data[i])
            {
                minTemp = outputVector.data[i]; // 가장 낮은 Q값을 가진 Action(= Element)를 찾는다.
                minOutputElement = i;
            }
        }

        float debugTemp0 = Mathf.Round((float)outputVector.data[0] * 100.0f) * 0.01f;
        float debugTemp1 = Mathf.Round((float)outputVector.data[1] * 100.0f) * 0.01f;
        float debugTemp2 = Mathf.Round((float)outputVector.data[2] * 100.0f) * 0.01f;

        Debug.Log("최적의 행동 : " + minOutputElement + "   Y0의   값은 : " + debugTemp0 +
                ",  Y1의   값은 : " + debugTemp1 + ",  Y2의   값은 : " + debugTemp2);
       
        if (isTraining)
            return Random.Range(0, 101) > 90 ? Random.Range(0, reinforcement.num_action) : minOutputElement;

        else
            return minOutputElement;
    }

}

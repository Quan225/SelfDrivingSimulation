using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reinforcement : MonoBehaviour {

    public int num_history; // 히스토리 개수
    public int num_stateVal; // 센서 개수
    public int num_action;  // 액션 개수
    
    public NeuralNetwork nn;

    public VectorArray oldInputVector;
    VectorArray nextInputVector;
    VectorArray rewardVector;

    public double maxRayDistance = 10.0;
    
    public double learningRate = 0.1;
    

    public void Initialize()
    {
        nn = new NeuralNetwork();
        

        const int num_HiddenLayer = 2;

        nn.Initialize(num_stateVal * num_history, num_action, num_HiddenLayer); // 뉴럴 네트워크 초기화 
                                                                                // 활성화 함수 바꾸고싶으면 NeuralNetwork에서 고칠것

        nn.alpha_ = learningRate; // Learning rate
        
        // ★ 리플레이 메모리는 일단 생략
    }



    public void forward(RayData curRayData) // 실행 시 입력벡터를 만들어 현재 네트워크에 입력을 진행하고, 출력값을 연산해둔 상태가 된다.
    {
        // history가 1이니 state들을 Vector로 만들어준다

        if(oldInputVector == null)  // oldInputVector가 비었다면
        {
            oldInputVector = new VectorArray(); // 생성한다.
            oldInputVector.Initialize(num_stateVal);
        }

        MakeInputVector(curRayData);

        nn.setInputVector( oldInputVector); // 인풋 벡터로 입력을 한다.
        nn.PropForward(); // feedFoward를 진행한다. 

        

    }


    public void MakeInputVector(RayData data)
    {
        for (int i = 0; i < num_stateVal; i++)
        {
            if(data.distanceData[i] > maxRayDistance)   // 너무 멀리까지 확인하지 않도록 한다.
            {
                data.distanceData[i] = maxRayDistance;
            }

            oldInputVector.data[i] = data.distanceData[i];    // 이번 행동으로 나온 데이터를 사용하여 input Vector에 사용될 벡터를 만들었다.
        }
    }


    //void MakeInputVector(int endIndex, VectorArray intputVector)
    //{
    //// 현재는 히스토리가 1이니 구현을 뒤로 미룬다.
    //}

}

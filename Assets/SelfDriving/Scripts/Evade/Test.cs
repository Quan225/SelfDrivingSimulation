using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Example
{
    public VectorArray x_, y_;
    public Example()
    {
        x_ = new VectorArray();
        y_ = new VectorArray();
    }
};


public class Test : MonoBehaviour {

    Example[] xorArr;

    public double target1;
    public double target2;

    VectorArray x;
    VectorArray y_target;
    VectorArray y_temp;
    NeuralNetwork nn_;

    // Use this for initialization
    void Start () {


        double startPos1 = 5.0;
        double startPos2 = 5.0;

        x = new VectorArray(2);
        x.data[0] = startPos1;
        x.data[1] = startPos2; // 초기 입력값

        y_target = new VectorArray(2);
        y_target.data[0] = 1;
        y_target.data[1] = 1;

        y_temp = new VectorArray(2);

        nn_ = new NeuralNetwork();
        nn_.Initialize(2, 2, 2);
        nn_.alpha_ = 0.1; // 요구하는 target의 값이 클수록 낮은 alpha를 요구함


        for (int i = 0; i < 10000; i++)
        {
            nn_.setInputVector(x);   // 입력 설정

            nn_.PropForward();

            nn_.CopyOutputVector( y_temp, false); // 출력값을 복사해서



            // Select Max Action, Update Game, get Reward, Back Prop
            if (y_temp.data[0] > y_temp.data[1])
            {   // 좌회전이 더 불리 우회전 선택
                x.data[0] += 1;
                x.data[1] -= 1;
            }
            else
            {       // 우회전이 더 불리 좌회전 선택
                x.data[0] -= 1;
                x.data[1] += 1;
            }


            //   get Reward, Back Prop

            if (x.data[0] <= 0)
            {   // 왼쪽에 부딪혔을 경우
                x.data[0] = startPos1;   // 위치 리셋
                x.data[1] = startPos2;


                y_target.data[0] = 2; // 0번에 벌점
                y_target.data[1] = 0.5; // 통상점수
                nn_.PropBackward( y_target);
                Debug.Log("왼박");
            }
            else if (x.data[1] <= 0)
            {   // 오른쪽에 부딪혔을 경우
                x.data[0] = startPos1;   // 위치 리셋
                x.data[1] = startPos2;


                y_target.data[0] = 0.5; // 0번에 벌점
                y_target.data[1] = 2; // 통상점수
                nn_.PropBackward( y_target);
                Debug.Log("오박");

            }



            if (i % 1000 == 0)
                Debug.Log(y_temp.data[0] + ", " + y_temp.data[1]);// 출력해본다.
            // Debug.Log("----------------------");
        }

        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            target1 -= 1;
            target2 += 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            target1 += 1;
            target2 -= 1;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            x.data[0] = target1;
            x.data[1] = target2;

            for (int i = 0; i < 1; i++)
            {
                nn_.setInputVector( x);   // 입력 설정

                nn_.PropForward();

                nn_.CopyOutputVector( y_temp, false); // 출력값을 복사해서
                Debug.Log("input is " + x.data[0] + ", " + x.data[1] );
                if(y_temp.data[0] > y_temp.data[1])
                {
                    Debug.Log("우회전");
                }
                else
                {
                    Debug.Log("좌회전");
                }

                // Debug.Log("----------------------");
            }
        }
    }
}

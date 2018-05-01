using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCtr : MonoBehaviour {

    public Test2 test2;

    Vector3 startPos;
    Quaternion startRot;
    public CarRay carRay;
    public float moveSpeed = 7.0f;
    public float turnSpeed = 30.0f;
    public double emergency = 5.0f;
    float speedControl = 1.0f;

    public Transform[] nextFrameAgent;
    public int checkFrameNum = 4;

    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        carRay = transform.GetComponent<CarRay>();
    }

    public void TurnRight()
    {

        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
    }

    public void TurnLeft()
    {

        transform.Rotate(Vector3.up * -turnSpeed * Time.deltaTime);
    }


    void CollTraining(ref VectorArray rewardVector, int curAction) // 충돌 시 벌점을 주는 트레이닝
    {
        for (int loop = 0; loop < carRay.rayCount; loop++)  // Sensor의 개수만큼 루프한다.
        {
            if (carRay.rayData.distanceData[loop] <= 1.0)  // 충돌했을 경우
            {
                for (int i = 0; i < test2.reinforcement.num_action; i++)
                {
                    if (curAction == i) // 충돌한 Action에게
                    {
                        rewardVector.data[i] = 2.0;   // 감점
                    }
                    else // 그 외에
                    {
                        rewardVector.data[i] = 0.5;   // 통상점수
                    }
                }
                test2.Training(rewardVector); // BackProp을 실행한다.

                transform.position = startPos;  // Position Reset
                transform.rotation = startRot;

                break;  // 다른 충돌을 계산할 필요는 없으니 빠져나간다.
            }

        }
    }

    void NearTraining(ref VectorArray rewardVector, int curAction) // 근접시 벌점, 충돌 시 큰 벌점을 주는 트레이닝
    {
        for (int loop = 0; loop < carRay.rayCount; loop++)  // Sensor의 개수만큼 루프한다.
        {

            if (carRay.rayData.distanceData[loop] <= 1.0)  // 충돌했을 경우
            {
                for (int i = 0; i < test2.reinforcement.num_action; i++)
                {
                    if (curAction == i) // 충돌한 Action에게
                    {
                        rewardVector.data[i] = 2.0;   // 감점
                    }
                    else // 그 외에
                    {
                        rewardVector.data[i] = 0.5;   // 통상점수
                    }
                }

                test2.Training(rewardVector); // BackProp을 실행한다.
                transform.position = startPos;  // Position Reset
                transform.rotation = startRot;
                break;  // 다른 충돌을 계산할 필요는 없으니 빠져나간다.
            }

            else if (carRay.rayData.distanceData[loop] <= 3.5)  // 충돌했을 경우
            {
                for (int i = 0; i < test2.reinforcement.num_action; i++)
                {
                    if (curAction == i) // 충돌한 Action에게
                    {
                        rewardVector.data[i] = 1.0;   // 감점
                    }
                    else // 그 외에
                    {
                        rewardVector.data[i] = 0.5;   // 통상점수
                    }
                }

                test2.Training(rewardVector); // BackProp을 실행한다.
            }

           

        }
    }

    void AlwaysTraining(ref VectorArray rewardVector, int curAction) // 충돌 시 벌점을 주는 트레이닝
    {
        for (int loop = 0; loop < carRay.rayCount; loop++)  // Sensor의 개수만큼 루프한다.
        {

            if (carRay.rayData.distanceData[loop] <= 1.0)  // 충돌했을 경우
            {
                for (int i = 0; i < test2.reinforcement.num_action; i++)
                {
                    if (curAction == i) // 충돌한 Action에게
                    {
                        rewardVector.data[i] = 2.0;   // 감점
                    }
                    else // 그 외에
                    {
                        rewardVector.data[i] = 0.5;   // 통상점수
                    }
                }

                test2.Training(rewardVector); // BackProp을 실행한다.
                transform.position = startPos;  // Position Reset
                transform.rotation = startRot;
                break;  // 다른 충돌을 계산할 필요는 없으니 빠져나간다.
            }

            else if (carRay.rayData.distanceData[loop] <= 3.5)  // 충돌했을 경우
            {
                for (int i = 0; i < test2.reinforcement.num_action; i++)
                {
                    if (curAction == i) // 충돌한 Action에게
                    {
                        rewardVector.data[i] = 1.0;   // 감점
                    }
                    else // 그 외에
                    {
                        rewardVector.data[i] = 0.5;   // 통상점수
                    }
                }

                test2.Training(rewardVector); // BackProp을 실행한다.
            }

            else if (carRay.rayData.distanceData[loop] <= 5.5)  // 충돌했을 경우
            {
                for (int i = 0; i < test2.reinforcement.num_action; i++)
                {
                    if (curAction == i) // 충돌한 Action에게
                    {
                        rewardVector.data[i] = 0.8;   // 감점
                    }
                    else // 그 외에
                    {
                        rewardVector.data[i] = 0.5;   // 통상점수
                    }
                }

                test2.Training(rewardVector); // BackProp을 실행한다.
            }

            else if (carRay.rayData.distanceData[loop] <= 7)  // 충돌했을 경우
            {
                for (int i = 0; i < test2.reinforcement.num_action; i++)
                {
                    if (curAction == i) // 충돌한 Action에게
                    {
                        rewardVector.data[i] = 0.6;   // 감점
                    }
                    else // 그 외에
                    {
                        rewardVector.data[i] = 0.5;   // 통상점수
                    }
                }

                test2.Training(rewardVector); // BackProp을 실행한다.
            }


        }
    }

    public void carUpdate(int action)
    {
        VectorArray reward = new VectorArray();
        reward.Initialize(3, true);


        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);  // Position Update
        switch (action)
        {
            case 0:
                break;
            case 1:
                TurnLeft();
                break;
            case 2:
                TurnRight();
                break;
        }

        carRay.UpdateRay(transform.position, true); // Sensor Update


        CollTraining(ref reward, action);


    }









    void EmergencyCheck()   // 코드가 너무 복잡해져서 빼줬음
    {
        for (int a = 0; a < carRay.rayCount; a++)
        {

            if (carRay.rayData.distanceData[a] < emergency)
            {
                speedControl = 0.5f;
            }
            else
            {
                speedControl = 1.0f;
            }
        }
    }

}







/* 
     앞쪽 센서에 뭐가 감지되었을 때 오른 벽 왼 벽 확인하도록 만들었더니 얘가 김여사가 된 코드.
     if(carRay.rayData.distanceData[2] <= emergency) // 만약 앞쪽 센서에 충돌위험이 떳다면.
     {
         switch (i)
         {
             case 0: // 현재 테스트가 직진일 경우
                 reward -= 1.0;  // 일단 직진이 선택되지는 않도록.
                 break;
             case 1: // 왼쪽
                 if(carRay.rayData.distanceData[1] <= emergency) // 왼쪽 앞에도 벽이 있을 경우
                 {
                     reward -= 0.1; 

                 }
                 if (carRay.rayData.distanceData[0] <= emergency) // 왼쪽에도 벽이 있을 경우
                 {
                     reward -= 0.15; 
                 }
                 break;
             case 2: // 오른쪽
                 if (carRay.rayData.distanceData[3] <= emergency) // 오른쪽 앞에도 벽이 있을 경우
                 {
                     reward -= 0.1;

                 }
                 if (carRay.rayData.distanceData[4] <= emergency) // 오른쪽에도 벽이 있을 경우
                 {
                     reward -= 0.15;
                 }
                 break;
         }

     }
     
    double rewardCheck(int i)   // i : 다음 프레임에서 했다고 가정한 행동의 num
    {
        double reward = 0.0;

Vector3 tempVec = transform.position;
Quaternion tempRot = transform.rotation;



        switch (i)
        {
            case 0:
                for (int j = 0; j<checkFrameNum* 2; j++)
                {
                    transform.Translate(Vector3.forward* moveSpeed * speedControl * Time.deltaTime);
                }
                break;
            case 1:
                for (int j = 0; j<checkFrameNum; j++)
                {
                    transform.Translate(Vector3.forward* moveSpeed * speedControl * Time.deltaTime);
TurnLeft();
                }
                break;
            case 2:
                for (int j = 0; j<checkFrameNum; j++)
                {
                    transform.Translate(Vector3.forward* moveSpeed * speedControl * Time.deltaTime);
TurnRight();
                }
                break;
        }
        nextFrameAgent[i].transform.position = transform.position;  // 더듬이 표시용.
        nextFrameAgent[i].transform.rotation = transform.rotation;

        carRay.UpdateRay(transform.position, false);    // 이동해본 후 RayUpdate



        reward = 2.00;




        for (int j = 0; j< 5; j++)
        {
            if (carRay.rayData.distanceData[j] <= emergency* 0.2)  // 벽과 충돌 => 큰 감점
            {
                reward -= 0.20;

            }
            else if (carRay.rayData.distanceData[j] <= emergency* 0.5) // 벽과 가까이 접근 => 감점
            {
                if (j == 2)
                {
                    reward -= 0.05;
                }
                else
                {
                    reward -= 0.03;

                }
            }
            else if (carRay.rayData.distanceData[j] <= emergency) // 벽과 접근 => 감점
            {
                if (j == 2)  // 정면의 센서가 지나친 접근을 할 경우
                {
                    reward -= 0.02;
                }
                else
                {
                    reward -= 0.015;

                }
            }
        }

        if (carRay.rayData.distanceData[0] - carRay.rayData.distanceData[4] < 0.5)  // 좌, 우 센서가 같은 거리를 띌 시
        {
            reward += 0.04;
        }

        transform.position = tempVec;   // 이동했었던 Agent를 원래 위치로 돌려준다.
        transform.rotation = tempRot;



        if (reward <= 0) // 비정상적인 Reward가 입력된 경우
        {
            reward = 1.0f;
        }

        return reward;
    }


    double rewardCheck2(int i)   // i : 다음 프레임에서 했다고 가정한 행동의 num
    {
        double reward = 0.0;

Vector3 tempVec = transform.position;
Quaternion tempRot = transform.rotation;



        switch (i)
        {
            case 0:
                for (int j = 0; j<checkFrameNum* 2; j++)
                {
                    transform.Translate(Vector3.forward* moveSpeed * speedControl * Time.deltaTime);
                }
                break;
            case 1:
                for (int j = 0; j<checkFrameNum; j++)
                {
                    transform.Translate(Vector3.forward* moveSpeed * speedControl * Time.deltaTime);
TurnLeft();
                }
                break;
            case 2:
                for (int j = 0; j<checkFrameNum; j++)
                {
                    transform.Translate(Vector3.forward* moveSpeed * speedControl * Time.deltaTime);
TurnRight();
                }
                break;
        }
        nextFrameAgent[i].transform.position = transform.position;  // 더듬이 표시용.
        nextFrameAgent[i].transform.rotation = transform.rotation;

        carRay.UpdateRay(transform.position, false);    // 이동해본 후 RayUpdate


        double minRayDistance = carRay.rayData.distanceData[0];

        for (int r = 1; r< 5; r++)
        {
            if (minRayDistance > carRay.rayData.distanceData[r])
            {
                minRayDistance = carRay.rayData.distanceData[r];
            }
        }


        reward = 1.00 * minRayDistance;



transform.position = tempVec;   // 이동했었던 Agent를 원래 위치로 돌려준다.
        transform.rotation = tempRot;


        return reward;
    }
    
     */

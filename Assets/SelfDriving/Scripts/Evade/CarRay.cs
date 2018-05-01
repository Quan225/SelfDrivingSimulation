using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayData
{
    public RayData()
    {
        distanceData = new double[5];
    }

    public RayData(int dataNum)
    {
        distanceData = new double[dataNum];
    }

    public double[] distanceData;
}


public class CarRay : MonoBehaviour {

    Ray[] ray;
    RaycastHit[] hit;

    public int rayCount = 5;
    public Transform[] tr;
    Vector3 yZeroPos;
    int layerMask;

    public RayData rayData;

    // Use this for initialization
    void Start ()
    {
        ray = new Ray[rayCount];
        hit = new RaycastHit[rayCount];
        rayData = new RayData(rayCount);

        layerMask = 1 << LayerMask.NameToLayer("Wall");
    }
	

    public RayData UpdateRay(Vector3 ratOrigin, bool debugLay)
    {

        Vector3 yZeroTemp1 = ratOrigin;
        yZeroTemp1.y = 0;

        ray[0].origin = yZeroTemp1;
        ray[1].origin = yZeroTemp1;
        ray[2].origin = yZeroTemp1;
        ray[3].origin = yZeroTemp1;
        ray[4].origin = yZeroTemp1;                                                        



        ray[0].direction = new Vector3(-transform.right.x, 0, -transform.right.z);  // 왼
        ray[1].direction = new Vector3(-transform.right.x + transform.forward.x, 0, -transform.right.z + transform.forward.z );  // 대각 왼
        ray[2].direction = new Vector3(transform.forward.x, 0, transform.forward.z);  // 앞
        ray[3].direction = new Vector3(transform.right.x + transform.forward.x, 0, transform.right.z  + transform.forward.z );  // 대각 오
        ray[4].direction = new Vector3(transform.right.x, 0, transform.right.z);  // 오

        for (int i = 0; i < 5; i++)
        {
            if (Physics.Raycast(ray[i], out hit[i], 600.0f, layerMask))
            {

              //  Debug.Log(i + " : " + hit[i].transform.name);
             

                if (debugLay)
                {
                    tr[i].position = hit[i].point;
                    Debug.DrawRay(transform.position, ray[i].direction * 600.0f, Color.red);    // 실제 Ray
                    Debug.DrawRay(transform.position, ray[i].direction * 10.0f, Color.green);   // network input
                   // Debug.DrawRay(transform.position, ray[i].direction * 5.0f, Color.blue);   // network input
                }

                Vector3 yZeroTemp2 = hit[i].point;

                yZeroTemp2.y = 0;

                rayData.distanceData[i] = (yZeroTemp1 - yZeroTemp2).magnitude;
            }
        }

        return rayData;
    }
    
}

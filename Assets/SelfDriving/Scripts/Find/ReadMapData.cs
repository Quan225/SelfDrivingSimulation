using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MapData{

    public MapData()
    {
        leftTop = Vector2.zero;
        rightBottom = Vector2.zero;
    }

    public MapData(float left, float top, float right, float bottom)
    {
        leftTop.x = left;
        leftTop.y = top;
        rightBottom.x = right;
        rightBottom.y = bottom;
    }

    public Vector2 leftTop; // 이 위치를 Pivot이라 생각한다.
    public Vector2 rightBottom;
    public float score;
}

public class ReadMapData : MonoBehaviour {

    public Transform startObj;
    public Transform targetObj;
    public Transform checkObj;

    public Vector2 startPos;
    public Vector2 targetPos;

    public float targetScore = 100.0f;


    public GameObject mapModel; // 맵 모델
    public float mapDefalutSizeX = 1.0f;   // 맵 모델의 scale (1x1x1) 상태에서의 x 크기 
    public float mapDefalutSizeY = 1.0f;
    public float mapSizeX = 0.0f;   // 스크립트에서 인식되는 맵의 크기.
    public float mapSizeY = 0.0f;

    float mapXStartPos;
    float mapXEndPos;
    float mapYStartPos;
    float mapYEndPos;

    public int arrSizeX;
    public int arrSizeY;

    MapData[][] mapDataArr;


    void Start()
    {
        if (mapModel != null)    // 맵 모델 데이터를 입력한 경우
        {
            mapSizeX = mapModel.transform.localScale.x * mapDefalutSizeX;
            mapSizeY = mapModel.transform.localScale.z * mapDefalutSizeY;
        }
        else
        {
            Debug.LogError("Please input Map Model GameObject");
        }

        mapXStartPos = -(mapSizeX * 0.5f);
        mapXEndPos = (mapSizeX * 0.5f);
        mapYStartPos = -(mapSizeY * 0.5f);
        mapYEndPos = (mapSizeY * 0.5f);

        mapDataArr = new MapData[arrSizeY][];
        for (int i = 0; i < arrSizeX; i++)
        {
            mapDataArr[i] = new MapData[arrSizeX];
        }


        float mapOneAreaSizeX = mapSizeX / (float)arrSizeX;
        float mapOneAreaSizeY = mapSizeY / (float)arrSizeY;

        for (int i = 0; i < arrSizeY; i++)
        {
            for (int j = 0; j < arrSizeX; j++)
            {
                mapDataArr[j][i] = new MapData();
                mapDataArr[j][i].leftTop.x = mapXStartPos + (mapOneAreaSizeX * j);
                mapDataArr[j][i].rightBottom.x = mapXStartPos + (mapOneAreaSizeX * j + 1);
                mapDataArr[j][i].leftTop.y = mapYStartPos + (mapOneAreaSizeY * i);
                mapDataArr[j][i].rightBottom.y = mapYStartPos + (mapOneAreaSizeY * i + 1);
            }
        }


        startPos.x = startObj.position.x;
        startPos.y = startObj.position.z;
        targetPos.x = targetObj.position.x;
        targetPos.y = targetObj.position.z;

        for (int i = 0; i < arrSizeY; i++)
        {
            for (int j = 0; j < arrSizeX; j++)
            {
                mapDataArr[j][i].score = targetScore - (Mathf.Abs(startPos.x - mapDataArr[j][i].leftTop.x) + Mathf.Abs(startPos.y - mapDataArr[j][i].leftTop.y));
            }

        }
    }





    MapData getMapData(int x, int y)
    {
        return mapDataArr[y][x];
    }
    MapData getMapData(Vector3 pos)
    {
        int posX = 0;
        int posY = 0;



        for (int i = 0; i < arrSizeX; i++)
        {
            if (pos.x < mapDataArr[posX][0].leftTop.x)
            {
                posX++;
            }
            else
            {
                break;
            }
        }
        for (int j = 0; j < arrSizeY; j++)
        {
            if (pos.y < mapDataArr[posY][0].leftTop.y)
            {
                posY++;
            }
            else
            {
                break;
            }

        }
        return mapDataArr[posX][posY];
    }

}

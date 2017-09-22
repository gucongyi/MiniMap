using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    private const float TerrainLength = 500f;
    private const float TerrainWidth = 500f;

    private const float MiniMapLength = 200f;
    private const float MiniMapWidth = 200f;

    private Vector3 TerrainCenterPoint;
    public Transform TerrainCenterPointTrans;
    public RectTransform MiniMapCenterTrans;

    public static MinimapController mInstance;

    void Awake()
    {
        mInstance = this;
    }

    // Use this for initialization
    void Start ()
    {
        TerrainCenterPoint = TerrainCenterPointTrans.position;
        ResourceManager.ResetDics();
    }

    public void UpdateMiniMapSelfPlayer(Transform goTrans,string iconName)
    {
        float TerrainDeltaX = goTrans.position.x - TerrainCenterPoint.x;
        float TerrainDeltaZ = goTrans.position.z - TerrainCenterPoint.z;

        float minimapDeltaX = TerrainDeltaX * MiniMapWidth / TerrainWidth;
        float minimapDeltaY = TerrainDeltaZ * MiniMapLength / TerrainLength;
        ResourceManager.LoadResourceAsync("MiniMapItem", completed: (GameObject go) =>
        {
            go.GetComponent<RectTransform>().SetParent(MiniMapCenterTrans);
            go.GetComponent<Image>().sprite = ResourceManager.LoadAsset("Textures", iconName, typeof(Sprite)) as Sprite;
            go.GetComponent<RectTransform>().anchoredPosition=new Vector2(minimapDeltaX, minimapDeltaY);
            go.GetComponent<RectTransform>().localScale=Vector3.one;
            go.GetComponent<RectTransform>().localEulerAngles=new Vector3(0f,0f, -goTrans.transform.localEulerAngles.y);//y转z,负的
        });
    }

    private Vector3 OffsetDrawCircleRoot=new Vector3(147.9f,0f, -25.39999f);
    public void UpdateMoveCircle(DrawCicle.Circle moveCircleInfo,string iconMinimapName)
    {
        float TerrainDeltaX = (moveCircleInfo.centerPoint+ OffsetDrawCircleRoot).x - TerrainCenterPoint.x;
        float TerrainDeltaZ = (moveCircleInfo.centerPoint+OffsetDrawCircleRoot).z - TerrainCenterPoint.z;
        float radius = moveCircleInfo.radius;
        //小地图圆心
        float minimapDeltaX = TerrainDeltaX * MiniMapWidth / TerrainWidth;
        float minimapDeltaY = TerrainDeltaZ * MiniMapLength / TerrainLength;

        //小地图半径，谁小用谁
        float minimapRadiusWidth = radius * MiniMapWidth / TerrainWidth;
        float minimapRadiusLength = radius * MiniMapLength / TerrainLength;
        float minMapRadius = (minimapRadiusWidth <= minimapRadiusLength) ? minimapRadiusWidth : minimapRadiusLength;

        ResourceManager.LoadResourceAsync("MiniMapCircle", completed: (GameObject go) =>
        {
            go.GetComponent<RectTransform>().SetParent(MiniMapCenterTrans);
            go.GetComponent<Image>().sprite = ResourceManager.LoadAsset("Textures", iconMinimapName, typeof(Sprite)) as Sprite;
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(minimapDeltaX, minimapDeltaY);
            go.GetComponent<RectTransform>().localScale = Vector3.one;
            go.GetComponent<RectTransform>().sizeDelta=new Vector2(minMapRadius*2, minMapRadius*2);
        });
    }

    // Update is called once per frame
	void Update () {
		
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class WallManager : MonoBehaviour
{
    public GameObject WallPrefab;
    private float _screenHeight;
    public GameplayManager Manager;
    private float Speed => Manager.GameplaySpeed;

    private Vector3 topRight;
    private Vector3 bottomLeft;

    [SerializeField] private float wallPosition;//y position of "active" wall
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        SpawnWall();
        wallPosition = _screenHeight;
    }

    private void Init()
    {
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth,Camera.main.pixelHeight,0));
        bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        _screenHeight = topRight.y - bottomLeft.y;
        //First wall
        var wall=SpawnWall();
        wall.transform.position = Vector3.zero;
    }
    public Transform SpawnWall()
    {
        var wall = GetWallObject();
        wall.transform.localScale = WallScale();
        wall.transform.position = new Vector3(0, topRight.y+_screenHeight/2);
        wallPosition = topRight.y + _screenHeight;
        //wall.transform.SetParent(transform);
        return wall.transform;
    }

    private void Update()
    {
        if (wallPosition <= 0)
        {
            SpawnWall();
            wallPosition = _screenHeight;
        }

        foreach (Transform child in transform)
        {
            //child.Translate(Vector3.down*Manager.GameplaySpeed*Time.deltaTime);
        }
        //wallPosition = wallPosition - Manager.GameplaySpeed*Time.deltaTime;
    }

    private Vector3 WallScale()
    {
        return new Vector3(1, _screenHeight*1.01f, 1);//little extra for overlaps
    }

    private GameObject GetWallObject()
    {
        return Instantiate(WallPrefab,transform);
    }
}

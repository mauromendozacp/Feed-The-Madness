using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLoop : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private GameObject[] floors = null;
    [SerializeField] private Killer player = null;
    [SerializeField] private float speed = 0f;
    #endregion

    #region PRIVATE_FIELDS
    private Vector3 playerPosition = Vector3.zero;
    private float lenghtZ = 65;
    #endregion

    #region PROPERTIES
    public GameObject[] Floors => floors;
    public float Speed => speed;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        playerPosition = player.transform.position;
    }

    private void Update()
    {
        MoveBack();
    }

    private void LateUpdate()
    {
        CheckFloor();
    }
    #endregion

    #region PRIVATE_METHODS
    private void CheckFloor()
    {
        foreach (GameObject obj in floors)
        {
            if (obj.transform.position.z + lenghtZ < playerPosition.z)
            {
                obj.transform.position =
                    new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z + lenghtZ * floors.Length);
            }
        }
    }

    private void MoveBack()
    {
        transform.Translate(-transform.forward * speed * Time.deltaTime);
    }
    #endregion
}

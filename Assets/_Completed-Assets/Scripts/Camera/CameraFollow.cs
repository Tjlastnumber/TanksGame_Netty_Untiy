using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    /// <summary>
    /// 相机跟随的速率
    /// </summary>
    [Tooltip("相机跟随的速率")]
    public float followSpeed = 1;
    /// <summary>
    /// 相机跟随的目标
    /// </summary>
    [Tooltip("相机跟随的目标")]
    public Transform player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (player == null) return;
        Transform cp = player.Find("CameraPoint");
        Vector3 pos = cp.position;
        transform.position = Vector3.Lerp(transform.position, cp.position, Time.deltaTime * followSpeed);
        transform.rotation = cp.rotation;
        //transform.LookAt(player.position);
    }
}

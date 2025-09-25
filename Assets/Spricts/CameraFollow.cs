using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("跟隨目標")]
    public Transform target;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("[SimpleFollow] 沒有指定目標");
            return;
        }

        // 只更新 x 軸，保持 y 和 z 不變
        Vector3 newPos = new Vector3(target.position.x, transform.position.y, transform.position.z);
        transform.position = newPos;
    }
}


using UnityEngine;
using System.Collections.Generic;

public class BoardProximityTriggerBox : MonoBehaviour
{
    [Header("矩形偵測範圍（半徑）")]
    public Vector3 boxHalfExtents = new Vector3(3f, 1f, 2f);
    // x = 左右一半寬
    // y = 高度一半
    // z = 前後一半長

    public LayerMask boardLayer;

    private HashSet<CollidedFall> activeBoards = new HashSet<CollidedFall>();

    void Update()
    {
        Collider[] hits = Physics.OverlapBox(
            transform.position,
            boxHalfExtents,
            Quaternion.identity,   // ★ 不跟著角色旋轉
            boardLayer
        );

        HashSet<CollidedFall> nearbyBoards = new HashSet<CollidedFall>();

        foreach (Collider hit in hits)
        {
            CollidedFall board = hit.GetComponent<CollidedFall>();
            if (board == null) continue;

            nearbyBoards.Add(board);

            if (!activeBoards.Contains(board))
            {
                board.StandUp();
                activeBoards.Add(board);
            }
        }

        List<CollidedFall> leftBoards = new List<CollidedFall>();
        foreach (var board in activeBoards)
        {
            if (!nearbyBoards.Contains(board))
            {
                board.FallDown();
                leftBoards.Add(board);
            }
        }

        foreach (var board in leftBoards)
        {
            activeBoards.Remove(board);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.matrix = Matrix4x4.TRS(
            transform.position,
            Quaternion.identity,   // ★ 固定方向
            Vector3.one
        );

        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);
    }
}
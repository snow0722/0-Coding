using UnityEngine;
using System.Collections.Generic;

public class BoardProximityTriggerOld : MonoBehaviour
{
    public float detectRadius = 3f;
    public LayerMask boardLayer; // 立牌所在圖層

    // 記錄目前已經立起的立牌，避免一直觸發
    private HashSet<CollidedFall> activeBoards = new HashSet<CollidedFall>();

    void Update()
    {
        // 找出玩家周圍所有立牌
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, boardLayer);

        // 新一輪靠近的立牌
        HashSet<CollidedFall> nearbyBoards = new HashSet<CollidedFall>();

        foreach (Collider hit in hits)
        {
            CollidedFall board = hit.GetComponent<CollidedFall>();
            if (board != null)
            {
                nearbyBoards.Add(board);

                // 之前沒立起的立牌，現在靠近立起
                if (!activeBoards.Contains(board))
                {
                    board.StandUp(); // 呼叫 public StandUp()
                    activeBoards.Add(board);
                }
            }
        }

        // 處理離開範圍的立牌
        // activeBoards 裡但不在 nearbyBoards 的立牌 → 離開範圍
        List<CollidedFall> leftBoards = new List<CollidedFall>();
        foreach (var board in activeBoards)
        {
            if (!nearbyBoards.Contains(board))
            {
                board.FallDown(); // 呼叫 public FallDown()
                leftBoards.Add(board);
            }
        }

        // 從 activeBoards 移除離開的立牌
        foreach (var board in leftBoards)
        {
            activeBoards.Remove(board);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : MonoBehaviour
{
   private int curX, curY;

   private readonly Vector2Int[] possibleDirection = new Vector2Int[]
   {
      new Vector2Int(1, 2), new Vector2Int(2, 1), 
      new Vector2Int(-1, 2), new Vector2Int(2, -1),  
      new Vector2Int(1, -2), new Vector2Int(-2, 1),
      new Vector2Int(-1, -2), new Vector2Int(-2, -1)
   };
   public void OnInitialized(int x, int y)
   {
      curX = x;
      curY = y;
   }

   public List<Vector2Int> GetPossibleMoves()
   {
      List<Vector2Int> possibleMoves = new List<Vector2Int>();
      for (int i = 0; i < possibleDirection.Length; i++)
      {
         int newX = curX + possibleDirection[i].x;
         int newY = curY + possibleDirection[i].y;
        
         if (GetBoundsCheck(newX, newY))
         {
            possibleMoves.Add(new Vector2Int(newX, newY));
         }
      }
      return possibleMoves;
   }


   private bool GetBoundsCheck(int x, int y)
   {
      return x >= 0 && x < 8 && y >= 0 && y < 8;
   }
}

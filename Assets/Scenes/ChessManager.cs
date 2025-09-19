using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessManager : MonoBehaviour
{
   [SerializeField] private GameObject tilePrefab;
   [SerializeField] private GameObject knightPrefab;

   [SerializeField] private int knightX, KnightY;
   
   private int boardSize = 8;
   private int tileSize = 1;


   private Tile[,] tileObjectList;
   private List<Tile> currentHighlights = new List<Tile>();
 
   private void Start()
   {
      CreateBoard();
      CreateKnightObjectOnBoard(knightX, KnightY);
   }

   private void CreateBoard()
   {
      tileObjectList = new Tile[boardSize, boardSize];
      
      for (int row = 0; row < boardSize; row++)
      {
         for (int col = 0; col < boardSize; col++)
         {
            Vector3 position = new Vector3(row, col, 0);
            GameObject tileObj = Instantiate(tilePrefab, position, Quaternion.identity, transform);
            tileObj.name = $"tile_{row}_{col}";
            Tile tile = tileObj.GetComponent<Tile>();
            tile?.OnInitialized(((row+col)%2==0)? Color.white : Color.black);

            tileObjectList[row, col] = tile;
         }
      }

      float boardcenterPos = tileSize * ((boardSize - 1) / 2);
      transform.position = new Vector3(-boardcenterPos, -boardcenterPos, 0);
   }

   private void CreateKnightObjectOnBoard(int x, int y)
   {

      GameObject knight = Instantiate(knightPrefab, tileObjectList[x, y].transform.position, Quaternion.identity, transform);
      knight.name = $"Knight_{x}_{y}";
      KnightMovement knightMV = knight.GetComponent<KnightMovement>();
      knightMV?.OnInitialized(x, y);

      List<Vector2Int> possibleMoves = knightMV.GetPossibleMoves();
      HighLightMoves(possibleMoves);
   }

   private void HighLightMoves( List<Vector2Int> possibleMoves)
   {
      if(currentHighlights.Count>0) 
         ClearHighlights();
      
      foreach (var mv in possibleMoves)
      {
         Tile tile = GetTile(mv.x, mv.y);
         if (tile != null)
         {
            tile.HighLight(Color.red);
            currentHighlights.Add(tile);
         }
      }
   }
   
   public void ClearHighlights()
   {
      foreach (var t in currentHighlights)
         t.DeHighlight();
      currentHighlights.Clear();
   }

   private Tile GetTile(int x, int y)
   {
      return tileObjectList[x, y];
   }
   
}

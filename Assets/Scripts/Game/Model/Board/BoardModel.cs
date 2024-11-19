using System.Collections.Generic;
using Game.View.Board;
using Game.Vo;
using UnityEngine;

namespace Game.Model.Board
{
  public class BoardModel : IBoardModel
  {
    public List<BoardSquareView> boardSquareViewList { get; set; }
    
    public BoardSquareView startSquare { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      boardSquareViewList = new List<BoardSquareView>();
    }

    public void ResetPosition(Transform playerTransform)
    {
      startSquare.PlaceInPosition(playerTransform);
    }
  }
}
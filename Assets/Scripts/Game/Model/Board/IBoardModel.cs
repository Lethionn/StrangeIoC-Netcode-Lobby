using System.Collections.Generic;
using Game.View.Board;
using Game.Vo;
using UnityEngine;

namespace Game.Model.Board
{
  public interface IBoardModel 
  {
    BoardSquareView startSquare { get; set; }
    
    List<BoardSquareView> boardSquareViewList { get; set; }
    
    void ResetPosition(Transform playerTransform);
  }
}
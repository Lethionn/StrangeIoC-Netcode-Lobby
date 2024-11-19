using System.Collections.Generic;
using strange.extensions.mediation.impl;

namespace Game.View.Board
{
  public class BoardControllerView : EventView
  {
    public BoardSquareView startSquareView;
    
    public List<BoardSquareView> boardSquareViewList;

    public void LeaveClick()
    {
      dispatcher.Dispatch(BoardControllerEvent.Leave);
    }
  }
}
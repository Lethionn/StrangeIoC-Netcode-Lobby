using Common.Enum;
using Common.Scene;
using Game.Model.Board;
using strange.extensions.mediation.impl;

namespace Game.View.Board
{
  public enum BoardControllerEvent
  { 
    Leave
  }
  
  public class BoardControllerMediator : EventMediator
  {
    [Inject]
    public BoardControllerView view { get; set; }

    [Inject]
    public IBoardModel boardModel { get; set; }
    
    public override void OnRegister()
    {
      view.dispatcher.AddListener(BoardControllerEvent.Leave, OnLeave);
    }

    public override void OnInitialize()
    {
      boardModel.boardSquareViewList.AddRange(view.boardSquareViewList);
      boardModel.startSquare = view.startSquareView;
    }
    
    private void OnLeave()
    {
      SceneLoader.Load(SceneKey.Online);
    }

    public override void OnRemove()
    {
      view.dispatcher.AddListener(BoardControllerEvent.Leave, OnLeave);
    }
  }
}
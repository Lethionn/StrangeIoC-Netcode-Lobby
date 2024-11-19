using System.Collections.Generic;
using Game.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Game.View.Board
{
  public class BoardSquareView : EventView
  {
    public BoardSquareVo vo;

    public List<Transform> slotList;
    
    private bool[] _slotOccupancyArray = { false, false, false, false, false };

    public void PlaceInPosition(Transform targetTransform)
    {
      for (int i = 0; i < _slotOccupancyArray.Length; i++)
      {
        bool slot = _slotOccupancyArray[i];
        if (slot) continue;
        
        targetTransform.position = slotList[i].position;
        _slotOccupancyArray[i] = true;
        break;
      }
    }
  }
}
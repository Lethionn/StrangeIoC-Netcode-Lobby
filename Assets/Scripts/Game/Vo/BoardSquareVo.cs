using System;
using System.Collections.Generic;
using Game.Enum;
using UnityEngine;

namespace Game.Vo
{
  [Serializable]
  public class BoardSquareVo
  {
    public int index;

    public Transform transform;

    public BoardSquareType type;
    
    public List<PlayerVo> playerList;
    
    public CreatureVo creature;
  }
}
using System;
using Unity.Collections;
using Unity.Netcode;

namespace Game.Vo
{
  public struct PlayerVo : IEquatable<PlayerVo>, INetworkSerializable
  {
    public ulong clientId;
    public FixedString64Bytes playerName;
    public FixedString64Bytes playerId;


    public bool Equals(PlayerVo other)
    {
      return
        clientId == other.clientId &&
        playerName == other.playerName &&
        playerId == other.playerId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref clientId);
      serializer.SerializeValue(ref playerName);
      serializer.SerializeValue(ref playerId);
    }
  }
}
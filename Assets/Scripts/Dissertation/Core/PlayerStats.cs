using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

namespace Dissertation.Core 
{
    [System.Serializable]
    public struct PlayerStats : IEquatable<PlayerStats>, INetworkSerializeByMemcpy
    {
        public int touches;
        public int piecesPutInPlace;

        public PlayerStats(int touches, int piecesPutInPlace) {
            this.touches = touches;
            this.piecesPutInPlace = piecesPutInPlace;
        }

        public bool Equals(PlayerStats other)
        {
            return other.touches == touches && other.piecesPutInPlace == piecesPutInPlace;
        }
    }
}

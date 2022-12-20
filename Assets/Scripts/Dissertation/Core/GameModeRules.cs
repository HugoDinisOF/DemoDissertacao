using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dissertation.BlockLogic;

namespace Dissertation.Core {
    [CreateAssetMenu(fileName = "GameModeRules", menuName = "ScriptableObjects/GameModeRules", order = 1)]
    public class GameModeRules : ScriptableObject
    {
        public int maxPlayerCount;
        public OwnershipType ownershipType;
        public List<PieceTypeCount> gamePieces;
        public float gameTime;
        
        // TODO: add score per piece

        public float timeToWin;

        public enum OwnershipType 
        {
            EVERYONE,
            COLOR,
            TURN_BASED,
        }

        [System.Serializable]
        public struct PieceTypeCount
        {
            public GameObject pieceObject;
            public Block block;
            public int count;
            public int playerId;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block
{
    public int id;
    public BlockColor color;
    public BlockType blockType;

    public enum BlockType 
    {
        CUBE,
    };

    public enum BlockColor
    {
        RED,
        BLUE,
    };

    public bool IsCompatible(Block block) {
        return block.color == color && block.blockType == blockType;
    }
}


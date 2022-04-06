using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MinecraftButScuffed.Rendering;

public class Chunk : GameComponent, IEnumerable<Block>, IComparable<Chunk>, IEquatable<Chunk>
{
    public const int Size = 2;
    public const int Height = 2;
    
    private readonly Block[][][] _blocks;
    public int X { get; }
    public int Y { get; }

    
    public Chunk(Vector2 position, Game game) : this((int)position.X, (int)position.Y, game)
    {
    }
    
    public Chunk(int x, int y, Game game) : base(game)
    {
        game.Components.Add(this);
        X = x;
        Y = y;
        _blocks = new Block[Size][][];
        for (var i = 0; i < Size; i++)
        {
            _blocks[i] = new Block[Height][];
            for (var j = 0; j < Height; j++)
            {
                _blocks[i][j] = new Block[Size];
                for (var k = 0; k < Size; k++)
                {
                    _blocks[i][j][k] = new Block(new Vector3(i, j, k), BlockType.Dirt, this, game);
                }
            }
        }
        
        /*
        _blocks[0][0][0] = new Block(new Vector3(X, 0, Y) * Size, BlockType.Grass, this, game);
        _blocks[1][0][0] = new Block(Vector3.Right + new Vector3(X, 0, Y) * Size, BlockType.Grass, this, game);
        _blocks[0][1][0] = new Block(Vector3.Up + new Vector3(X, 0, Y) * Size, BlockType.Grass, this, game);
        _blocks[0][0][1] = new Block(new Vector3(X, 0, Y) * Size + Vector3.Backward, BlockType.Grass, this, game);
    */
    }

    public IEnumerator<Block> GetEnumerator()
    {
        return _blocks.SelectMany(x => x).SelectMany(x => x).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int CompareTo(Chunk other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        int xComparison = X.CompareTo(other.X);
        if (xComparison != 0) return xComparison;
        return Y.CompareTo(other.Y);
    }

    public bool Equals(Chunk other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(_blocks, other._blocks) && X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Chunk) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_blocks, X, Y);
    }
    
    public Block this[int x, int y, int z]
    {
        get
        {
            if (x is < 0 or >= Size || y is < 0 or >= Height || z is < 0 or >= Size)
                return null;
            return _blocks[x][y][z];
        }
        set => _blocks[x][y][z] = value;
    }

    public Block this[Vector3 position]
    {
        get
        {
            if (position.X is < 0 or >= Size || position.Y is < 0 or >= Height || position.Z is < 0 or >= Size)
                return null;
            return _blocks[(int) position.X][(int) position.Y][(int) position.Z];
        }
        set => _blocks[(int) position.X][(int) position.Y][(int) position.Z] = value;
    }
}
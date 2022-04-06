using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MinecraftButScuffed.Rendering;

public class World : GameComponent, IEnumerable<Chunk>
{
    public const int Chunks = 16;
    
    private readonly Chunk[][] _chunks = new Chunk[Chunks][];

    public World(Game game) : base(game)
    {
        game.Components.Add(this);
        for (int x = 0; x < Chunks; x++)
        {
            _chunks[x] = new Chunk[Chunks];
            for (int y = 0; y < Chunks; y++)
                _chunks[x][y] = new Chunk(x, y, game);
        }
    }
    
    public Chunk this[Vector2 position] => _chunks[(int)position.X][(int)position.Y];
    public Chunk this[int x, int y] => _chunks[x][y];
    public IEnumerator<Chunk> GetEnumerator()
    {
        return _chunks.SelectMany(x => x).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
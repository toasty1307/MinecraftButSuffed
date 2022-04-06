using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;

namespace MinecraftButScuffed.Rendering;

public class Block : DrawableGameComponent
{
    public Vector3 Position;
    public readonly BlockType Type;
    public readonly Chunk Chunk;
    private BasicEffect _effect;
    private Camera _camera;
    private Texture2D _dirt;
    private Texture2D[] _textures;
    private Texture2D _grass;

    public Block(Vector3 position, BlockType type, Chunk chunk, Game game) : base(game)
    {
        ((MinecraftButScuffed) game).DrawThings.Add(this);
        Position = position;
        Type = type;
        Chunk = chunk;
    }

    public override void Initialize()
    {
        _camera = Game.Services.GetService<Camera>();
        _effect = _camera.Effect;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _dirt = Game.Content.Load<Texture2D>("dirt");
        _grass = Game.Content.Load<Texture2D>("grass");
        //                 Front,  Back,  Left,  Right,  Top,  Bottom
        _textures = new[] {_dirt, _dirt, _dirt, _dirt, _grass, _dirt};
    }

    public override unsafe void Draw(GameTime gameTime)
    {
        if (Type is BlockType.Air) return;

        var corners = stackalloc Vector3[8];
        corners[0] = Position;
        corners[1] = Position + new Vector3(2, 0, 0);
        corners[2] = Position + new Vector3(2, 2, 0);
        corners[3] = Position + new Vector3(0, 2, 0);
        corners[4] = Position + new Vector3(0, 0, 2);
        corners[5] = Position + new Vector3(2, 0, 2);
        corners[6] = Position + new Vector3(2, 2, 2);
        corners[7] = Position + new Vector3(0, 2, 2);


        var inFrustum = false;
        
        for (var i = 0; i < 8; i++)
        {
            var corner = corners[i];
            var dot = Vector3.Dot(corner - _camera.Position, _camera.LookAt - _camera.Position);
            if (dot < 0) continue;
            inFrustum = true;
            break;
        }
        
        if (!inFrustum)
            return;

        _effect.World = Matrix.CreateTranslation(Position);
        
        var indices = new short[]
        {
            0, 1, 2,
            0, 2, 3
        };
        
        // idc if it can be made into a loop

        VertexPositionTexture[] vertices = null;
        
        #region Front face
        
        var renderFrontFace = false;
        
        var neighbor = Chunk[(int)Position.X % 16, (int)Position.Y % 16, (int) (Position.Z - 1) % 16];
        if (neighbor is {Type: BlockType.Air} or null)
        {
            renderFrontFace = true;
        }

        if (renderFrontFace)
        {
            vertices = new VertexPositionTexture[]
            {
                new(corners[0], new Vector2(0, 0)),
                new(corners[1], new Vector2(1, 0)),
                new(corners[2], new Vector2(1, 1)),
                new(corners[3], new Vector2(0, 1)),
            };

            _effect.Texture = _textures[0];

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0,
                    2);
            }
        }

        #endregion

        #region Back face

        var renderBackFace = false;
        
        neighbor = Chunk[(int)Position.X % 16, (int)Position.Y % 16, (int) (Position.Z + 1) % 16];
        if (neighbor is {Type: BlockType.Air} or null)
        {
            renderBackFace = true;
        }

        if (renderBackFace)
        {

            vertices = new VertexPositionTexture[]
            {
                new(corners[4], new Vector2(0, 0)),
                new(corners[5], new Vector2(1, 0)),
                new(corners[6], new Vector2(1, 1)),
                new(corners[7], new Vector2(0, 1)),
            };

            _effect.Texture = _textures[1];

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0,
                    2);
            }
        }

        #endregion
        
        #region Left face
        
        var renderLeftFace = false;
        
        neighbor = Chunk[(int) (Position.X - 1) % 16, (int)Position.Y % 16, (int)Position.Z % 16];
        if (neighbor is {Type: BlockType.Air} or null)
        {
            renderLeftFace = true;
        }
        
        if (renderLeftFace)
        {
            vertices = new VertexPositionTexture[]
            {
                new(corners[0], new Vector2(0, 0)),
                new(corners[4], new Vector2(1, 0)),
                new(corners[7], new Vector2(1, 1)),
                new(corners[3], new Vector2(0, 1)),
            };

            _effect.Texture = _textures[2];

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0,
                    2);
            }
        }
        
        #endregion
        
        #region Right face
        
        var renderRightFace = false;
        
        neighbor = Chunk[(int) (Position.X + 1) % 16, (int)Position.Y % 16, (int)Position.Z % 16];
        if (neighbor is {Type: BlockType.Air} or null)
        {
            renderRightFace = true;
        }
        
        if (renderRightFace)
        {
            vertices = new VertexPositionTexture[]
            {
                new(corners[1], new Vector2(0, 0)),
                new(corners[2], new Vector2(1, 0)),
                new(corners[6], new Vector2(1, 1)),
                new(corners[5], new Vector2(0, 1)),
            };

            _effect.Texture = _textures[3];

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0,
                    2);
            }
        }
        
        #endregion
        
        #region Top face
        
        var renderTopFace = false;
        
        neighbor = Chunk[(int)Position.X % 16, (int) (Position.Y + 1) % 16, (int)Position.Z % 16];
        if (neighbor is {Type: BlockType.Air} or null)
        {
            renderTopFace = true;
        }
        
        if (renderTopFace)
        {
            vertices = new VertexPositionTexture[]
            {
                new(corners[3], new Vector2(0, 0)),
                new(corners[2], new Vector2(1, 0)),
                new(corners[6], new Vector2(1, 1)),
                new(corners[7], new Vector2(0, 1)),
            };

            _effect.Texture = _textures[4];

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0,
                    2);
            }
        }
        
        #endregion
        
        #region Bottom face
        
        var renderBottomFace = false;
        
        neighbor = Chunk[(int)Position.X % 16, (int) (Position.Y - 1) % 16, (int)Position.Z % 16];
        if (neighbor is {Type: BlockType.Air} or null)
        {
            renderBottomFace = true;
        }
        
        if (renderBottomFace)
        {
            vertices = new VertexPositionTexture[]
            {
                new(corners[1], new Vector2(0, 0)),
                new(corners[0], new Vector2(1, 0)),
                new(corners[4], new Vector2(1, 1)),
                new(corners[5], new Vector2(0, 1)),
            };

            _effect.Texture = _textures[5];

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0,
                    2);
            }
        }
        
        #endregion
    }
}
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinecraftButScuffed.Rendering;

public class Camera : DrawableGameComponent, IContentLoader
{
    public Matrix View { get; private set; }
    public Matrix Projection { get; private set; }

    private BasicEffect _effect;

    public float FieldOfView
    {
        get => _fieldOfView;
        private set
        {
            _fieldOfView = value;
            UpdateProjectionMatrix();
        }
    }

    public float NearPlane
    {
        get => _nearPlane;
        private set
        {
            _nearPlane = value;
            UpdateProjectionMatrix();
        }
    }
    
    public float FarPlane
    {
        get => _farPlane;
        private set
        {
            _farPlane = value;
            UpdateProjectionMatrix();
        }
    }

    public Vector3 LookAt
    {
        get => _lookAt;
        set
        {
            _lookAt = value;
            UpdateViewMatrix();
        }
    }

    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            UpdateViewMatrix();
        }
    }

    private float _fieldOfView = 60;
    private float _nearPlane = 0.1f;
    private float _farPlane = 1000f;
    private Vector3 _lookAt = Vector3.Forward * 100;
    private Vector3 _position = Vector3.Zero;
    private GraphicsDevice _graphicsDevice;
    private VertexBuffer _vertexBuffer;
    private GameManager _gameManager;

    public Camera(Game game) : base(game)
    {
        DrawOrder = (int) Rendering.DrawOrder.World;
        Game.Components.Add(this);
    }

    public override void Initialize()
    {
        base.Initialize();
        
        UpdateProjectionMatrix();
        UpdateViewMatrix();
        
        _graphicsDevice = Game.GraphicsDevice;
        
        _effect = new BasicEffect(_graphicsDevice);
        _effect.Alpha = 1.0f;
        _effect.VertexColorEnabled = true;

        _vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
        
        _vertexBuffer.SetData(new[]
        {
            new VertexPositionColor(new Vector3(  0,  20, 0), Color.Red),
            new VertexPositionColor(new Vector3(-20, -20, 0), Color.Green),
            new VertexPositionColor(new Vector3( 20, -20, 0), Color.Blue)
        });

        _gameManager = Game.Services.GetService<GameManager>();
    }
    
    private void UpdateProjectionMatrix()
    {
        Projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(_fieldOfView),
            Game.GraphicsDevice.DisplayMode.AspectRatio,
            _nearPlane, 
            _farPlane
        );
    }
    
    private void UpdateViewMatrix()
    {
        View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);
    }

    public override void Draw(GameTime gameTime)
    {
        if (_gameManager.CurrentGameState is not GameState.Playing) return;
        
        _effect.Projection = Projection;
        _effect.View = View;
        _effect.World = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
        
        _graphicsDevice.SetVertexBuffer(_vertexBuffer);

        var rasterizerState = new RasterizerState();
        rasterizerState.CullMode = CullMode.None;
        _graphicsDevice.RasterizerState = rasterizerState;
        
        foreach (var pass in _effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
        }
    }

    public void LoadContent(SpriteBatch spriteBatch)
    {
    }

    public int LoadContentOrder => (int) Rendering.DrawOrder.World; // cry
}
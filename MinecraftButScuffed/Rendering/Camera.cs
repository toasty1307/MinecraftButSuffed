using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Serilog;

namespace MinecraftButScuffed.Rendering;

public class Camera : DrawableGameComponent
{
    public Matrix View { get; private set; }
    public Matrix Projection { get; private set; }

    public BasicEffect Effect { get; private set; }

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
    private float _nearPlane = 0.001f;
    private float _farPlane = 100000f;
    private Vector3 _lookAt = Vector3.Zero;
    private Vector3 _position = Vector3.Forward * 10;
    private GraphicsDevice _graphicsDevice;
    private Point _lastMousePosition;

    public Camera(Game game) : base(game)
    {
        DrawOrder = (int) Rendering.DrawOrder.World;
        Game.Components.Add(this);
        Game.Services.AddService(this);
        _ = new World(game);
    }

    public override void Initialize()
    {
        base.Initialize();
        
        _graphicsDevice = Game.GraphicsDevice;
        
        Effect = new BasicEffect(_graphicsDevice);
        Effect.Alpha = 1.0f;
        Effect.VertexColorEnabled = false;

        Effect.TextureEnabled = true;

        var rasterizerState = new RasterizerState();
        rasterizerState.CullMode = CullMode.None;
        
        _graphicsDevice.RasterizerState = rasterizerState;

        _graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        
        UpdateProjectionMatrix();
        UpdateViewMatrix();
    }
    
    private void UpdateProjectionMatrix()
    {
        Projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(_fieldOfView),
            Game.GraphicsDevice.DisplayMode.AspectRatio,
            _nearPlane, 
            _farPlane
        );
        if (Effect != null)
        {
            Effect.Projection = Projection;
        }
    }

    private void UpdateViewMatrix()
    {
        View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);
        
        if (Effect != null)
        {
            Effect.View = View;
        }
    }

    public override void Update(GameTime gameTime)
    {
        var deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

        var movement = Vector3.Zero;
        var state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.W))
        {
            movement.Z += 1;
        }

        if (state.IsKeyDown(Keys.S))
        {
            movement.Z -= 1;
        }

        if (state.IsKeyDown(Keys.A))
        {
            movement.X += 1;
        }

        if (state.IsKeyDown(Keys.D))
        {
            movement.X -= 1;
        }
        
        if (state.IsKeyDown(Keys.Space))
        {
            movement.Y += 1;
        }
        
        if (state.IsKeyDown(Keys.LeftShift))
        {
            movement.Y -= 1;
        }
        
        if (movement == Vector3.Zero) return;
        
        var speed = deltaTime * 10;
        Position += new Vector3(movement.X, movement.Y, movement.Z) * speed;
    }
}
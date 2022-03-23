using Microsoft.Xna.Framework;

namespace MinecraftButScuffed.Rendering;

public class Camera : Component
{
    public Matrix View { get; private set; }
    public Matrix Projection { get; private set; }

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
    private Vector3 _lookAt = Vector3.Forward;
    private Vector3 _position = Vector3.Zero;

    public Camera(Game game) : base(game)
    {
    }

    public override void Initialize()
    {
        UpdateProjectionMatrix();
        UpdateViewMatrix();
    }
    
    private void UpdateProjectionMatrix()
    {
        Projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(_fieldOfView),
            Game.GraphicsDevice.Viewport.AspectRatio,
            _nearPlane, 
            _farPlane
        );
    }
    
    private void UpdateViewMatrix()
    {
        View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);
    }
}
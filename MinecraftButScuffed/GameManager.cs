using Microsoft.Xna.Framework;
using MinecraftButScuffed.Rendering;

namespace MinecraftButScuffed;

public class GameManager : GameComponent
{
    private readonly UiManager _uiManger;
    private readonly Camera _camera;

    public GameState CurrentGameState { get; private set; }
    
    public GameManager(Game game) : base(game)
    {
        game.Components.Add(this);
        game.Services.AddService(this);
        _uiManger = new UiManager(game);
        _camera = new Camera(game);
    }
    
    public override void Initialize()
    {
        CurrentGameState = GameState.Playing;
    }
}
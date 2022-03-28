using Microsoft.Xna.Framework;

namespace MinecraftButScuffed.Rendering;

public class UiManager : GameComponent
{
    private readonly LevelGeneratorUi _levelGeneratorUi;
    private readonly GameManager _gameManager;

    public UiManager(Game game) : base(game)
    {
        game.Components.Add(this);
        _levelGeneratorUi = new LevelGeneratorUi(game);
        _gameManager = game.Services.GetService<GameManager>();
    }
}
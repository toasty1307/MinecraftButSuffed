using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinecraftButScuffed.Extensions;

namespace MinecraftButScuffed.Rendering;

public class LevelGeneratorUi : DrawableGameComponent
{
    private Texture2D _dirt;
    private SpriteFont _font;
    private Texture2D _whiteRectangle;
    private SpriteBatch _spriteBatch;
    private GameManager _gameManager;
    
    public LevelGeneratorUi(Game game) : base(game)
    {
        DrawOrder = (int) Rendering.DrawOrder.Gui;
        Game.Components.Add(this);
    }

    public override void Initialize()
    {
        base.Initialize();
        _gameManager = Game.Services.GetService<GameManager>();
    }

    protected override void LoadContent()
    {
        _spriteBatch = spriteBatch;
        _dirt = Game.Content.Load<Texture2D>("dirt");
        _font = Game.Content.Load<SpriteFont>("MinecraftFont");
        _whiteRectangle = new Texture2D(Game.GraphicsDevice, 200, 4);
        _whiteRectangle.SetData(Enumerable.Repeat(Color.White, 800).ToArray());
    }

    public override void Draw(GameTime gameTime)
    {
        if (_gameManager.CurrentGameState != GameState.GeneratingLevel) return;

        var width = Game.GraphicsDevice.Viewport.Width;
        var height = Game.GraphicsDevice.Viewport.Height;
        var numY = 8;
        var scale = height / 8f / 16f;
        var numX = width / (int) (scale * 16) + 1;

        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp
        );
        
        var backgroundColor = new Color(70, 70, 70, 255);
        
        for (var x = 0; x < numX; x++)
        for (var y = 0; y < numY; y++)
        {
            var position = new Vector2(x * 16, y * 16) * scale;
            
            _spriteBatch.Draw(
                _dirt, position, null,
                backgroundColor, 0f, new Vector2(0, 0),
                scale, SpriteEffects.None, 0f
            );
        }

        var screenCenter = new Vector2(width, height) / 2;

        var fontScale = height / 480f;
        _spriteBatch.DrawText("Generating level", screenCenter - new Vector2(0, 33) * fontScale, _font, fontScale);

        _spriteBatch.DrawText("Eroding..", screenCenter + new Vector2(0, 15.4F) * fontScale, _font, fontScale);

        var pos = screenCenter + new Vector2(0, 35.2F) * fontScale;
        var origin = new Vector2(100, 2);
        _spriteBatch.Draw(
            _whiteRectangle, pos, null,
            new Color(128, 128, 128, 255), 0f, origin, 
            fontScale, SpriteEffects.None, 0
        );
        
        _spriteBatch.Draw(
            _whiteRectangle, pos, null,
            new Color(128, 255, 128, 255), 0f, origin,
            fontScale, SpriteEffects.None, 0
        );

        _spriteBatch.End();
    }
}
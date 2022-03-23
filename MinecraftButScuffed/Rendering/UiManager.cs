using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinecraftButScuffed.Rendering;

public class UiManager : Component
{
    private SpriteBatch _spriteBatch;
    private Texture2D _dirt;
    private SpriteFont _font;
    private Texture2D _whiteRectangle;
    private readonly Vector2 _textShadowOffset = Vector2.One * 2;
    private readonly Color _textShadowOffset2 = new Color(63, 63, 63, 255);

    public UiManager(Game game) : base(game)
    {
        DrawOrder = (int) Rendering.DrawOrder.Gui;
    }

    public void LoadContent(SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
        _dirt = Game.Content.Load<Texture2D>("dirt");
        _font = Game.Content.Load<SpriteFont>("MinecraftFont");
        _whiteRectangle = new Texture2D(Game.GraphicsDevice, 200, 4);
        _whiteRectangle.SetData(Enumerable.Repeat(Color.White, 800).ToArray());
    }

    public override void Draw(GameTime gameTime)
    {
        var width = Game.GraphicsDevice.Viewport.Width;
        var height = Game.GraphicsDevice.Viewport.Height;
        var numX = width / _dirt.Width / 4 + 1;
        var numY = height / _dirt.Height / 4 + 1;

        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp
        );
        
        var backgroundColor = new Color(70, 70, 70, 255);
        
        for (var x = 0; x < numX; x++)
        for (var y = 0; y < numY; y++)
        {
            var position = new Vector2(x * _dirt.Width, y * _dirt.Height) * 4;
            
            _spriteBatch.Draw(
                _dirt, position, null,
                backgroundColor, 0f, Vector2.Zero,
                4.0f, SpriteEffects.None, 0f
            );
        }

        var screenCenter = new Vector2(Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height) / 2;

        DrawText("Generating level", screenCenter -Vector2.UnitY * 33, Color.White);

        DrawText("Eroding..", screenCenter + Vector2.UnitY * 15.4F, Color.White);

        _spriteBatch.Draw(
            _whiteRectangle, screenCenter + Vector2.UnitY * 35.2F, null,
            new Color(128, 128, 128, 255), 0f, new Vector2(100, 2), 
            1f, SpriteEffects.None, 0
        );
        
        _spriteBatch.Draw(
            _whiteRectangle, screenCenter + Vector2.UnitY * 35.2F, null,
            new Color(128, 255, 128, 255), 0f, new Vector2(100, 2),
            1f, SpriteEffects.None, 0
        );

        _spriteBatch.End();
    }
    
    public void DrawText(string text, Vector2 position, Color color)
    {
        var origin = _font.MeasureString(text) / 2;
        _spriteBatch.DrawString(
            _font, text, position + _textShadowOffset,
            _textShadowOffset2, 0, origin,
            1.0f, SpriteEffects.None, 0.5f
        );
        
        _spriteBatch.DrawString(
            _font, text, position,
            color, 0, origin,
            1.0f, SpriteEffects.None, 0.5f
        );
    }
}
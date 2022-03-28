using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinecraftButScuffed.Extensions;

public static class SpriteBatchExtensions
{
    private static readonly Vector2 _shadowOffset = new(2, 2);
    private static readonly Color _shadowColor = new(63, 63, 63, 255);
    
    public static void DrawText(this SpriteBatch spriteBatch, string text, Vector2 position, SpriteFont font, float scale = 1.0f)
    {
        var origin = font.MeasureString(text) / 2;
        spriteBatch.DrawString(
            font, text, position + _shadowOffset,
            _shadowColor, 0, origin,
            scale, SpriteEffects.None, 0.5f
        );
        
        spriteBatch.DrawString(
            font, text, position,
            Color.White, 0, origin,
            scale, SpriteEffects.None, 0.5f
        );
    }
    
    public static void DrawText(this SpriteBatch spriteBatch, string text, Vector2 position, SpriteFont font, Color color, float scale = 1.0f)
    {
        var origin = font.MeasureString(text) / 2;
        spriteBatch.DrawString(
            font, text, position + _shadowOffset,
            _shadowColor, 0, origin,
            scale, SpriteEffects.None, 0.5f
        );
        
        spriteBatch.DrawString(
            font, text, position,
            color, 0, origin,
            scale, SpriteEffects.None, 0.5f
        );
    }
    
    public static void DrawText(this SpriteBatch spriteBatch, string text, Vector2 position, SpriteFont font, Color color, Vector2 shadowOffset, float scale = 1.0f)
    {
        var origin = font.MeasureString(text) / 2;
        spriteBatch.DrawString(
            font, text, position + shadowOffset,
            _shadowColor, 0, origin,
            scale, SpriteEffects.None, 0.5f
        );
        
        spriteBatch.DrawString(
            font, text, position,
            color, 0, origin,
            scale, SpriteEffects.None, 0.5f
        );
    }
    
    public static void DrawText(this SpriteBatch spriteBatch, string text, Vector2 position, SpriteFont font, Color color, Vector2 shadowOffset, Color shadowColor, float scale = 1.0f)
    {
        var origin = font.MeasureString(text) / 2;
        spriteBatch.DrawString(
            font, text, position + shadowOffset,
            shadowColor, 0, origin,
            scale, SpriteEffects.None, 0.5f
        );
        
        spriteBatch.DrawString(
            font, text, position,
            color, 0, origin,
            scale, SpriteEffects.None, 0.5f
        );
    }
}
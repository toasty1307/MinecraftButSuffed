using System;
using Microsoft.Xna.Framework;
using IDrawable = MinecraftButScuffed.Rendering.IDrawable;

namespace MinecraftButScuffed;

public class Component : GameComponent, IDrawable
{
    private int _drawOrder;

    public Component(Game game) : base(game)
    {
        game.Components.Add(this);
    }

    public virtual void Draw(GameTime gameTime)
    {
    }

    public event EventHandler<EventArgs> DrawOrderChanged;

    public int DrawOrder
    {
        get => _drawOrder;
        set
        {
            if (_drawOrder == value)
                return;
            _drawOrder = value;
            OnDrawOrderChanged(this, EventArgs.Empty);
        }
    }
    
    /// <summary>
    /// Called when <see cref="P:MinecraftButScuffed.Component.DrawOrder" /> changed. Raises the <see cref="E:MinecraftButScuffed.Component.DrawOrderChanged" /> event.
    /// </summary>
    /// <param name="sender">This <see cref="T:MinecraftButScuffed.Component" />.</param>
    /// <param name="args">Arguments to the <see cref="E:MinecraftButScuffed.Component.DrawOrderChanged" /> event.</param>
    protected virtual void OnDrawOrderChanged(object sender, EventArgs args)
    {
        DrawOrderChanged?.Invoke(sender, args);
    }
}
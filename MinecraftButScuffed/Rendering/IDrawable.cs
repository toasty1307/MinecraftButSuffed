using System;
using Microsoft.Xna.Framework;

namespace MinecraftButScuffed.Rendering;

public interface IDrawable
{
    /// <summary>
    /// Called when this <see cref="T:Microsoft.Xna.Framework.IDrawable" /> should draw itself.
    /// </summary>
    /// <param name="gameTime">The elapsed time since the last call to <see cref="M:Microsoft.Xna.Framework.IDrawable.Draw(Microsoft.Xna.Framework.GameTime)" />.</param>
    void Draw(GameTime gameTime);

    /// <summary>
    /// Raised when <see cref="P:Microsoft.Xna.Framework.IDrawable.UpdateOrder" /> changed.
    /// </summary>
    event EventHandler<EventArgs> DrawOrderChanged;

    /// <summary>
    /// The draw order of this <see cref="T:Microsoft.Xna.Framework.GameComponent" /> relative
    /// to other <see cref="T:Microsoft.Xna.Framework.GameComponent" /> instances.
    /// </summary>
    int DrawOrder { get; }
}

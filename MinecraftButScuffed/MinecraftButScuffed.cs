using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinecraftButScuffed.Rendering;
using IDrawable = MinecraftButScuffed.Rendering.IDrawable;

namespace MinecraftButScuffed;

public class MinecraftButScuffed : Game
{
    public GraphicsDeviceManager Graphics;
    public SpriteBatch SpriteBatch;
    public UiManager UiManager;
    public readonly SortedList<int, IDrawable> Drawables = new();

    public MinecraftButScuffed()
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        Components.ComponentAdded += ComponentAdded;
        UiManager = new UiManager(this);
        
        var graphicsDeviceViewport = Graphics.GraphicsDevice.Viewport;
        graphicsDeviceViewport.Width = 854;
        graphicsDeviceViewport.Height = 480;
        Graphics.GraphicsDevice.Viewport = graphicsDeviceViewport;
        Graphics.PreferredBackBufferWidth = graphicsDeviceViewport.Width;
        Graphics.PreferredBackBufferHeight = graphicsDeviceViewport.Height;
        Graphics.ApplyChanges();

        base.Initialize();
    }

    private void ComponentAdded(object sender, GameComponentCollectionEventArgs e)
    {
        if (e.GameComponent is IDrawable drawable)
            Drawables.Add(drawable.DrawOrder, drawable);
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        Services.AddService(SpriteBatch);
        UiManager.LoadContent(SpriteBatch);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        foreach (var drawable in Drawables)
        {
            drawable.Value.Draw(gameTime);
        }
        
        base.Draw(gameTime);
    }
}
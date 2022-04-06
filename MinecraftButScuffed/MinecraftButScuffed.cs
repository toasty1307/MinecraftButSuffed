using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinecraftButScuffed;

public class MinecraftButScuffed : Game
{
    public List<DrawableGameComponent> DrawThings = new();
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameManager _gameManager;

    public MinecraftButScuffed()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        _gameManager = new GameManager(this);
    }

    protected override void Initialize()
    {
        var graphicsDeviceViewport = _graphics.GraphicsDevice.Viewport;
        
        graphicsDeviceViewport.Width = 854;
        graphicsDeviceViewport.Height = 480;
        
        _graphics.GraphicsDevice.Viewport = graphicsDeviceViewport;
        _graphics.PreferredBackBufferWidth = graphicsDeviceViewport.Width;
        _graphics.PreferredBackBufferHeight = graphicsDeviceViewport.Height;
        _graphics.ApplyChanges();
        
        base.Initialize();
        
        DrawThings.ForEach(d => d.Initialize());
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Services.AddService(_spriteBatch);
        
        base.LoadContent();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        foreach (var drawableGameComponent in DrawThings)
        {
            drawableGameComponent.Draw(gameTime);
        }
        
        base.Draw(gameTime);
    }
}
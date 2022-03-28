using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinecraftButScuffed.Rendering;

namespace MinecraftButScuffed;

public class MinecraftButScuffed : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private readonly SortedList<int, IContentLoader> _contentLoaders = new();
    private GameManager _gameManager;

    public MinecraftButScuffed()
    {
        Components.ComponentAdded += ComponentAdded;
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
    }

    private void ComponentAdded(object sender, GameComponentCollectionEventArgs e)
    {
        if (e.GameComponent is IContentLoader loader)
            _contentLoaders.Add(loader.LoadContentOrder, loader);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Services.AddService(_spriteBatch);
        
        foreach (var loader in _contentLoaders.Values)
            loader.LoadContent(_spriteBatch);
        
        base.LoadContent();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        base.Draw(gameTime);
    }
}
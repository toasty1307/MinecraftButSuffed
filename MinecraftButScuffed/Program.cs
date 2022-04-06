﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Serilog;

namespace MinecraftButScuffed;

public static class Program
{
    [STAThread]
    private static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

        using var game = new MinecraftButScuffed();
        game.Run();
    }
}

public class Test3DDemo : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    //Camera
    Vector3 camTarget;
    Vector3 camPosition;
    Matrix projectionMatrix;
    Matrix viewMatrix;
    Matrix worldMatrix;

    //BasicEffect for rendering
    BasicEffect basicEffect;

    //Geometric info
    VertexPositionColor[] triangleVertices;
    VertexBuffer vertexBuffer;

    //Orbit
    bool orbit = false;

    public Test3DDemo()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        base.Initialize();

        //Setup Camera
        camTarget = new Vector3(0f, 0f, 0f);
        camPosition = new Vector3(0f, 0f, -100f);
        projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45f),
            GraphicsDevice.DisplayMode.AspectRatio,
            1f, 1000f);
        viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
            new Vector3(0f, 1f, 0f)); // Y up
        worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);

        //BasicEffect
        basicEffect = new BasicEffect(GraphicsDevice);
        basicEffect.Alpha = 1f;

        basicEffect.VertexColorEnabled = true;

        basicEffect.LightingEnabled = false;

        //Geometry  - a simple triangle about the origin
        triangleVertices = new VertexPositionColor[3];
        triangleVertices[0] = new VertexPositionColor(new Vector3(
            0, 20, 0), Color.Red);
        triangleVertices[1] = new VertexPositionColor(new Vector3(-
            20, -20, 0), Color.Green);
        triangleVertices[2] = new VertexPositionColor(new Vector3(
            20, -20, 0), Color.Blue);

        //Vert buffer
        vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(
            VertexPositionColor), 3, BufferUsage.WriteOnly);
        vertexBuffer.SetData<VertexPositionColor>(triangleVertices)
            ;
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void UnloadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
            ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            camPosition.X -= 1f;
            camTarget.X -= 1f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            camPosition.X += 1f;
            camTarget.X += 1f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            camPosition.Y -= 1f;
            camTarget.Y -= 1f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            camPosition.Y += 1f;
            camTarget.Y += 1f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
        {
            camPosition.Z += 1f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
        {
            camPosition.Z -= 1f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            orbit = !orbit;
        }

        if (orbit)
        {
            Matrix rotationMatrix = Matrix.CreateRotationY(
                MathHelper.ToRadians(1f));
            camPosition = Vector3.Transform(camPosition,
                rotationMatrix);
        }

        viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
            Vector3.Up);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        basicEffect.Projection = projectionMatrix;
        basicEffect.View = viewMatrix;
        basicEffect.World = worldMatrix;

        GraphicsDevice.Clear(Color.CornflowerBlue);
        GraphicsDevice.SetVertexBuffer(vertexBuffer);

        RasterizerState rasterizerState = new RasterizerState();
        rasterizerState.CullMode = CullMode.None;
        GraphicsDevice.RasterizerState = rasterizerState;

        foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
        }

        base.Draw(gameTime);
    }
}
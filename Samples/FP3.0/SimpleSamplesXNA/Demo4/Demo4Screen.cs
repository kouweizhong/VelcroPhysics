﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using DemoBaseXNA;
using DemoBaseXNA.DrawingSystem;
using DemoBaseXNA.ScreenSystem;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FarseerGames.SimpleSamplesXNA.Demo4
{
    /// <summary>
    /// Designed to show the new rendering engine.
    /// </summary>
    public class Demo4Screen : GameScreen
    {
        List<Body> _crateBodies = new List<Body>();

        QuadRenderEngine renderEngine;
        float crateSize = 0.4f;
        Stopwatch watch;
        Random rand = new Random();
        int Count = 45;
        int BodyCount;

        public override void Initialize()
        {
            PhysicsSimulator = new World(new Vector2(0, -9.8f), true);

            Vertices box = PolygonTools.CreateBox(crateSize, crateSize);
            PolygonShape shape = new PolygonShape(box, 5);

            //Vector2 x = new Vector2(-7.0f, 0.75f);
            //Vector2 deltaX = new Vector2(0.5625f, 1.25f);
            //Vector2 deltaY = new Vector2(1.125f, 0.0f);

            Vector2 x = new Vector2(-15f, -18);
            Vector2 deltaX = new Vector2(0.45f, .8f);
            Vector2 deltaY = new Vector2(.9f, 0.0f);

            for (int i = 0; i < Count; ++i)
            {
                Vector2 y = x;

                for (int j = i; j < Count; ++j)
                {
                    Body body = PhysicsSimulator.CreateBody();
                    _crateBodies.Add(body);
                    body.BodyType = BodyType.Dynamic;
                    body.Position = y;
                    body.CreateFixture(shape);

                    y += deltaY;

                    BodyCount++;
                }

                x += deltaX;
            }

            // init the new render engine
            renderEngine = new QuadRenderEngine(ScreenManager.GraphicsDevice);

            renderEngine.Submit(ScreenManager.ContentManager.Load<Texture2D>("Content/Crate"), true);

            watch = new Stopwatch();

            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {

            watch.Start();
            // add a quad
            foreach (var globe in _crateBodies)
            {
                Color tint;

                if (globe.Awake == true)
                    tint = Color.White;
                else
                    tint = Color.LightBlue;

                renderEngine.Submit(new Quad(globe.Position, globe.Rotation,
                    (crateSize + 0.0055f) * 2, (crateSize + 0.0055f) * 2, 0, tint));
            }

            renderEngine.Render();
            watch.Stop();

            ScreenManager.SpriteBatch.Begin();

            ScreenManager.SpriteBatch.DrawString(ScreenManager.SpriteFonts.DiagnosticSpriteFont, watch.ElapsedMilliseconds.ToString(), new Vector2(5, 0), Color.Black);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.SpriteFonts.DiagnosticSpriteFont, Stopwatch.Frequency.ToString(), new Vector2(50, 0), Color.Black);

            ScreenManager.SpriteBatch.End();

            watch.Reset();


            base.Draw(gameTime);
        }

        public override void HandleInput(InputState input)
        {
            if (firstRun)
            {
                ScreenManager.AddScreen(new PauseScreen(GetTitle(), GetDetails()));
                firstRun = false;
            }

            if (input.PauseGame)
            {
                ScreenManager.AddScreen(new PauseScreen(GetTitle(), GetDetails()));
            }

            if (input.CurrentKeyboardState.IsKeyDown(Keys.Space))
            {
                ScreenManager.Game.IsFixedTimeStep = true;
            }
            else
            {
                //ScreenManager.Game.IsFixedTimeStep = false;
            }

            base.HandleInput(input);
        }

        public static string GetTitle()
        {
            return "Graphics Demo: Cool new interface";
        }

        public static string GetDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("This demo shows a single body with geometry");
            sb.AppendLine("attached.");
            sb.AppendLine(string.Empty);
            sb.AppendLine("GamePad:");
            sb.AppendLine("  -Rotate: left and right triggers");
            sb.AppendLine("  -Move: left thumbstick");
            sb.AppendLine(string.Empty);
            sb.AppendLine("Keyboard:");
            sb.AppendLine("  -Rotate: left and right arrows");
            sb.AppendLine("  -Move: A,S,D,W");
            return sb.ToString();
        }
    }
}
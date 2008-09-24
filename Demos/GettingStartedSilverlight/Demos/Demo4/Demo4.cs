﻿using System.IO;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
using FarseerGames.FarseerPhysics.Mathematics;
using FarseerSilverlightDemos.Demos.DemoShare;

namespace FarseerSilverlightDemos.Demos.Demo4
{
    public class Demo4 : SimulatorView
    {
        private Agent agent;
        private Border border;
        private Floor floor;
        private Pyramid pyramid;

        private int pyramidBaseBodyCount = 8;
        private Body rectangleBody;
        private Geom rectangleGeom;

        public Demo4()
        {
            Initialize();
            forceAmount = 1000;
            torqueAmount = 14000;
        }

        public override string Title
        {
            get { return "Stacked Objects"; }
        }

        public override string Details
        {
            get
            {
                StringWriter sb = new StringWriter();
                sb.Write("This demo shows the stacking stability of the engine.");
                sb.Write(" It shows a stack of rectangular bodies stacked in");
                sb.WriteLine(" the shape of a pyramid.");
                sb.WriteLine("");
                sb.WriteLine("Keyboard:");
                sb.WriteLine("  -Rotate : K and L");
                sb.WriteLine("  -Move : A,S,D,W");
                sb.WriteLine("");
                sb.WriteLine("Mouse:");
                sb.WriteLine("  -Hold down left button and drag");
                return sb.ToString();
            }
        }

        public override void Initialize()
        {
            ClearCanvas();
            physicsSimulator = new PhysicsSimulator(new Vector2(0, 100));
            physicsSimulator.BiasFactor = .4f;
            int borderWidth = (int) (ScreenManager.ScreenHeight*.05f);
            border = new Border(ScreenManager.ScreenWidth + borderWidth*2, ScreenManager.ScreenHeight + borderWidth*2,
                                borderWidth, ScreenManager.ScreenCenter);
            border.Load(this, physicsSimulator);
            rectangleBody = BodyFactory.Instance.CreateRectangleBody(32, 32, 1f);
            rectangleGeom = GeomFactory.Instance.CreateRectangleGeom(rectangleBody, 32, 32);
            rectangleGeom.FrictionCoefficient = .4f;
            rectangleGeom.RestitutionCoefficient = 0f;

            //create the pyramid near the bottom of the screen.
            pyramid = new Pyramid(rectangleBody, rectangleGeom, 32f/3f, 32f/3f, 32, 32, pyramidBaseBodyCount,
                                  new Vector2(ScreenManager.ScreenCenter.X - pyramidBaseBodyCount*.5f*(32 + 32/3),
                                              ScreenManager.ScreenHeight - 125));
            pyramid.Load(this, physicsSimulator);

            floor = new Floor(ScreenManager.ScreenWidth, 100,
                              new Vector2(ScreenManager.ScreenCenter.X, ScreenManager.ScreenHeight - 50));
            floor.Load(this, physicsSimulator);

            agent = new Agent(ScreenManager.ScreenCenter - new Vector2(320, 300));
            agent.Load(this, physicsSimulator);
            controlledBody = agent.Body;
            base.Initialize();
        }
    }
}
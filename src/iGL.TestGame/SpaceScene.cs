﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.Math;
using iGL.TestGame.GameObjects;

namespace iGL.TestGame
{
    public class SpaceScene : Scene
    {
        private OrthographicCamera _testCamera;
        private Sphere _light;
        private float _alpha = 7;

        public SpaceScene() : base(new Physics2d())
        {
            this.OnTick += new EventHandler<Engine.Events.TickEvent>(SpaceScene_OnTick);
        }

        void SpaceScene_OnTick(object sender, Engine.Events.TickEvent e)
        {
            _light.Position = new Vector3((float)(Math.Cos(_alpha / 2.0f) * 20.0f), (float)(Math.Sin(_alpha / 2.0f) * 10.0f) + 10.0f, (float)(Math.Sin(_alpha / 2.0f) * 20.0f));

            //if (_alpha < 10)
            //{
            //    _testCamera.Position = new Vector3(0.0f, _alpha, 40.0f);
            //    _testCamera.CameraComponent.Target = new Vector3(0, _alpha, 0);
            //}

            _alpha += e.Elapsed * 1.0f;

        }

        public override void Load()
        {
            /* create camera */

            _testCamera = new OrthographicCamera()
            {
                Height = 20.0f,
                Width = 20.0f * (3.0f / 2.0f),
                ZNear = 1.00f,
                ZFar = 1000.0f
            };

            _testCamera.Position = new Vector3(10.0f, 5, 20.0f);
            _testCamera.CameraComponent.Target = new Vector3(0, 5, 0);
            _testCamera.CameraComponent.ClearColor = new Vector4(0.2f, 0.2f, 0.2f, 1);

            AmbientColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            
            AddGameObject(_testCamera);
            SetCurrentCamera(_testCamera);
            
            var pointlight = new PointLight();
            pointlight.Ambient = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            pointlight.Diffuse = new Vector4(1.0f, 1.0f, 0.8f, 1.0f);

            _light = new Sphere() { Scale = new Vector3(2.0f) };
            _light.Position = new Vector3(10, 10, 0);
            _light.AddComponent(new LightComponent(pointlight));
            //_light.AddComponent(new SphereColliderComponent());
            //_light.AddComponent(new RigidBodyComponent());
            _light.Material.Ambient = new Vector4(1, 1, 0, 1);

            AddGameObject(_light);
            SetCurrentLight(_light);

            //var world = new Sphere(10.0f, 32, 32);
            //world.Position = new Vector3(0, 0, 0);
            //world.Material.Diffuse = new Vector4(0.5f, 0.5f, 0.5f, 1);
            //world.AddComponent(new SphereColliderComponent());          

            //world.AddComponent(new RigidBodyComponent(mass: 100, isStatic: true));

            //AddGameObject(world);

            //var world2 = new Sphere(3.0f, 32, 32);
            //world2.Position = new Vector3(0, 10, 0);
            //world2.Material.Diffuse = new Vector4(0.5f, 0.5f, 0.5f, 1);
            //world2.AddComponent(new SphereColliderComponent());

            //world2.AddComponent(new RigidBodyComponent(mass: 500, isStatic: true, isGravitySource:true));

            //AddGameObject(world2);

            //var world3 = new Sphere(2.0f, 32, 32);
            //world3.Position = new Vector3(-10, 15, 0);
            //world3.Material.Diffuse = new Vector4(0.5f, 0.5f, 0.5f, 1);
            //world3.AddComponent(new SphereColliderComponent());

            //world3.AddComponent(new RigidBodyComponent(mass: 200, isStatic: true, isGravitySource:true));

            //AddGameObject(world3);

            var slingShot = new SlingShot();
            slingShot.Position = new Vector3(0, 5, 0);
            AddGameObject(slingShot);

            //var c = new Cube(1, 1, 1);
            //c.Position = new Vector3(0, 100, 0);
            //c.Rotation = new Vector3(0, 0, 10);
            //c.OnMouseIn += (a, b) => ((Cube)a).Material.Ambient = new Vector4(0.5f, 0.5f, 0.5f, 1);
            //c.OnMouseOut += (a, b) => ((Cube)a).Material.Ambient = new Vector4(1.0f, 0.5f, 0.5f, 1);

            //((Cube)cube).Material.Diffuse = new Vector4(0.5f, 0.5f, 0.5f, 1);
            //((Cube)cube).Material.Ambient = new Vector4(0.5f, 0.5f, 0.5f, 1);

            //AddGameObject(c);

            AddTimer(new Timer()
            {
                Action = () =>
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            var cube = new Cube() { Scale = new Vector3(0.5f, 0.5f, 0.5f) };
                            cube.Position = new Vector3(-2.5f + i / 2.0f, 20, 0);
                            cube.Material.Diffuse = new Vector4(1.0f, 0.0f, 0.0f, 1);
                            cube.AddComponent(new BoxColliderComponent());
                            cube.AddComponent(new RigidBodyComponent());

                            cube.OnMouseIn += new EventHandler<Engine.Events.MouseInEvent>(cube_OnMouseIn);
                            cube.OnMouseOut += new EventHandler<Engine.Events.MouseOutEvent>(cube_OnMouseOut);
                            cube.OnMouseDown += new EventHandler<Engine.Events.MouseButtonDownEvent>(cube_OnMouseDown);
                            cube.OnMouseUp += new EventHandler<Engine.Events.MouseButtonUpEvent>(cube_OnMouseUp);
                            AddGameObject(cube);
                        }

                        for (int i = 0; i < 10; i++)
                        {
                            var sphere = new Sphere() { Scale = new Vector3( 0.5f ), Rings = 8, Segments = 8 };
                            sphere.Position = new Vector3(-2.5f + i / 2.0f, 30, 0);
                            sphere.Material.Diffuse = new Vector4(1.0f, 1.0f, 0.0f, 1);
                            sphere.AddComponent(new SphereColliderComponent());
                            sphere.AddComponent(new RigidBodyComponent());

                            AddGameObject(sphere);
                        }
                    },

                Interval = TimeSpan.FromSeconds(5),
                Mode = Timer.TimerMode.Once
            });
        }

        void cube_OnMouseUp(object sender, Engine.Events.MouseButtonUpEvent e)
        {
            var cube = sender as Cube;
            cube.Material.Diffuse = new Vector4(1.0f, 0.0f, 0.0f, 1);
        }

        void cube_OnMouseDown(object sender, Engine.Events.MouseButtonDownEvent e)
        {
            var cube = sender as Cube;
            cube.Material.Diffuse = new Vector4(0.0f, 1.0f, 0.0f, 1);
        }

        void cube_OnMouseOut(object sender, Engine.Events.MouseOutEvent e)
        {
            var cube = sender as Cube;
            cube.Material.Diffuse = new Vector4(1.0f, 0.0f, 0.0f, 1);
        }

        void cube_OnMouseIn(object sender, Engine.Events.MouseInEvent e)
        {
            var cube = sender as Cube;
            cube.Material.Diffuse = new Vector4(0.0f, 1.0f, 1.0f, 1);
        }
    }

}

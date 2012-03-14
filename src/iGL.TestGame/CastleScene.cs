using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.Math;
using iGL.TestGame.GameObjects;

namespace iGL.TestGame
{
    public class CastleScene : Scene
    {
        private Camera _testCamera;
        private Castle _castle;

        private float alpha = 0;

        public CastleScene()
        {
            this.OnTick += new EventHandler<Engine.Events.TickEvent>(CastleScene_OnTick);
        }

        void CastleScene_OnTick(object sender, Engine.Events.TickEvent e)
        {
            _testCamera.Position = new Vector3((float)(Math.Cos(alpha / 2.0f) * 40.0f), (float)(Math.Sin(alpha / 2.0f) * 5.0f) + 15.0f, (float)(Math.Sin(alpha / 2.0f) * 40.0f));

            alpha += e.Elapsed * 1.0f;
        }

        public override void Load()
        {
            var camProperties = new PerspectiveProperties()
            {
                AspectRatio = MathHelper.DegreesToRadians(45.0f),
                FieldOfViewRadians = 3.0f / 2.0f,
                ZNear = 1.00f,
                ZFar = 1000.0f
            };

            _testCamera = new Camera(camProperties);
            _testCamera.Position = new Vector3(40.0f, 10.0f, 80.0f);
            _testCamera.CameraComponent.Target = new Vector3(0, 0, 0);
            AddGameObject(_testCamera);
            SetCurrentCamera(_testCamera);

            ShaderProgram.SetAmbientColor(new Vector4(0.5f, 0.5f, 0.5f, 1.0f));

            /* add a point light */

            var pointlight = new PointLight();
            pointlight.Ambient = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            pointlight.Diffuse = new Vector4(1.0f, 1.0f, 0.8f, 1.0f);

            /* add some test cubes */

            var testCube = new Cube(2.0f, 2.0f, 2.0f);
            testCube.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
            testCube.Material.Diffuse = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);
            testCube.Position = new Vector3(20.0f, 20.0f, 0.0f);

            testCube.AddComponent(new LightComponent(pointlight));

            AddGameObject(testCube);

            SetCurrentLight(testCube);

            _castle = new Castle();
            _castle.SetDefaultStones();

            AddGameObject(_castle);
            _castle.Build(new Vector3(-10, 0, -10));

            //_castle = new Castle();
            //_castle.SetDefaultStones();

            //AddGameObject(_castle);
            //_castle.Build(new Vector3(10, 0, 10));

            //_castle = new Castle();
            //_castle.SetDefaultStones();

            //AddGameObject(_castle);
            //_castle.Build(new Vector3(-10, 0, 10));

            //_castle = new Castle();
            //_castle.SetDefaultStones();

            //AddGameObject(_castle);
            //_castle.Build(new Vector3(10, 0, -10));

            //for (int i = 0; i < 50; i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //        var sphere = new Sphere(1.0f);
            //        sphere.AddComponent(new SphereColliderComponent(sphere));

            //        var rigidBody = new RigidBodyComponent(sphere);
            //        rigidBody.Mass = 50.0f;

            //        sphere.AddComponent(rigidBody);
            //        sphere.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
            //        sphere.Material.Diffuse = new Vector4(0.0f, 0.0f, 1.0f, 0.0f);
            //        sphere.Position = new Vector3(-i, 5 + i * 2, -i);

            //        AddGameObject(sphere);
            //    }
            //    else
            //    {
            //        var cube = new Cube(2.0f, 2.0f, 2.0f);
            //        cube.AddComponent(new BoxColliderComponent(cube));

            //        var rigidBody = new RigidBodyComponent(cube);
            //        rigidBody.Mass = 50.0f;

            //        cube.AddComponent(rigidBody);
            //        cube.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
            //        cube.Material.Diffuse = new Vector4(0.0f, 0.5f, 1.0f, 0.0f);
            //        cube.Position = new Vector3(i, 5 + i * 2, i);

            //        AddGameObject(cube);
            //    }
            //}
        }
    }
}

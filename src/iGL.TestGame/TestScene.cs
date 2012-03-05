using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.Math;

namespace iGL.TestGame
{
    public class TestScene : Scene
    {
        private Cube _testCube;
        private Camera _testCamera;
        private float alpha = 0;//(float)Math.PI * 2.0f; 

        public TestScene()
        {
            this.OnTick += new EventHandler<Engine.Events.TickEvent>(TestScene_OnTick);
        }

        void TestScene_OnTick(object sender, Engine.Events.TickEvent e)
        {
            _testCamera.Position = new Vector3((float)(Math.Cos(alpha / 2.0f) * 30.0f), (float)(Math.Sin(alpha / 2.0f) * 10.0f) + 10.0f, (float)(Math.Sin(alpha / 2.0f) * 30.0f));

            alpha += e.Elapsed * 1.0f;
        }

        public override void Load()
        {
            /* create camera */

            _testCamera = new Camera(MathHelper.DegreesToRadians(45.0f), 3.0f / 2.0f, 1.00f, 1000.0f);
            _testCamera.Position = new Vector3(50.0f, 1.5f, 0.0f);
            _testCamera.CameraComponent.Target = new Vector3(0, 1.5f, 0);

            AddGameObject(_testCamera);
            SetCurrentCamera(_testCamera);

            ShaderProgram.SetAmbientColor(new Vector4(0.5f, 0.5f, 0.5f, 1.0f));

            /* add a point light */

            var pointlight = new PointLight();
            pointlight.Ambient = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            pointlight.Diffuse = new Vector4(1.0f, 1.0f, 0.8f, 1.0f);

            var lightObj = new GameObject();
            lightObj.Position = new Vector3(0, 100, 0);
            lightObj.AddComponent(new LightComponent(lightObj, pointlight));

            AddGameObject(lightObj);
            SetCurrentLight(lightObj);


            int nCubes = 100;
            for (int i = 0; i < nCubes; i++)
            {
                var cube = new Cube(1.0f, 1.0f, 1.0f);
                cube.Position = new Vector3(0, i + 0.5f, i / 100.0f);
                cube.AddComponent(new BoxColliderComponent(cube));
                cube.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
                cube.Material.Diffuse = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                cube.AddComponent(new RigidBodyComponent(cube));

                AddGameObject(cube);

                if (i + 1 == nCubes)
                {
                    cube.AddComponent(new LightComponent(cube, pointlight));
                    SetCurrentLight(cube);
                }

                var sphere = new Sphere(0.5f);
                sphere.Position = new Vector3(4, i * 2 + 5, i / 200.0f);
                sphere.AddComponent(new BoxColliderComponent(sphere));
                sphere.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
                sphere.Material.Diffuse = new Vector4(1.0f, 1.0f, 0.0f, 1.0f);              
                sphere.AddComponent(new RigidBodyComponent(sphere));

                AddGameObject(sphere);
            }

            var floor = new Cube(50.0f, 10.0f, 50.0f);
            floor.Position = new Vector3(0.0f, -5.0f, 0.0f);
            floor.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
            floor.Material.Diffuse = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);            

            floor.AddComponent(new BoxColliderComponent(floor));
            floor.AddComponent(new RigidBodyComponent(floor, isStatic: true, mass: 10.0f));
         
            AddGameObject(floor);

            //var c = new Cube(3.0f, 3.0f, 3.0f);
            //c.Position = new Vector3(0, 11f, 0.5f);
            //c.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
            //c.Material.Diffuse = new Vector4(1.0f, 0.5f, 0.5f, 1.0f);
            ////c.Rotation = new Vector3((float)(Math.PI / 5.5f), 0, 0);

            //c.AddComponent(new BoxColliderComponent(c));
            //c.AddComponent(new RigidBodyComponent(c, mass: 10.0f));

            //AddGameObject(c);
        }

        //private GameObject TestCompound(Vector3 position, Vector3 rotation)
        //{
        //    var cubes = new List<GameObject>();

        //    for (int i = 0; i < 10; i++)
        //    {
        //        var cube = new Cube(1.0f, 1.0f, 1.0f);
        //        cube.Position = new Vector3(i, 0, 0);
        //        cube.AddComponent(new BoxColliderComponent(cube));
        //        cube.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
        //        cube.Material.Diffuse = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
        //        cubes.Add(cube);
        //    }           

        //    var compoundObject = new CompoundObject( cubes , 50.0f);
        //    compoundObject.Position = position;
        //    compoundObject.Rotation = rotation;

        //    AddGameObject(compoundObject);

        //    return compoundObject;
        //}
    }
}

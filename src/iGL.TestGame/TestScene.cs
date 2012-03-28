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
        private GameObject cube;
        private PerspectiveCamera _testCamera;
        private float alpha = 0;//(float)Math.PI * 2.0f; 
        private float angle = (float)Math.PI / 8.0f;
        private int _sizeX = 10;
        private int _sizeY = 10;

        public TestScene()
            : base(new Physics2d())
        {
            this.OnTick += new EventHandler<Engine.Events.TickEvent>(TestScene_OnTick);
        }

        void TestScene_OnTick(object sender, Engine.Events.TickEvent e)
        {
            //_testCamera.Position = new Vector3((float)(Math.Cos(alpha / 2.0f) * 10.0f), (float)(Math.Sin(alpha / 2.0f) * 10.0f) + 10.0f, (float)(Math.Sin(alpha / 2.0f) * 10.0f));
            //cube.Rotation = new Vector3(0, 0, alpha);
            alpha += e.Elapsed * 0.1f;

            //Console.WriteLine(alpha);
        }

        public override void Load()
        {                       

            var pointlight = new PointLight();
            pointlight.Ambient = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            pointlight.Diffuse = new Vector4(1.0f, 1.0f, 0.8f, 1.0f);

            //var _light = new Sphere(1.0f);
            //_light.Position = new Vector3(0, 20, 0);
            //_light.AddComponent(new LightComponent(pointlight));         
            //_light.Material.Ambient = new Vector4(1, 1, 0, 1);

            //AddGameObject(_light);

            //SetCurrentLight(_light);

            _testCamera = new PerspectiveCamera()
            {
                FieldOfViewRadians = MathHelper.DegreesToRadians(45.0f),
                AspectRatio = 3.0f / 2.0f,
                ZNear = 1.00f,
                ZFar = 1000.0f
            };

            _testCamera.Position = new Vector3(0.0f, 100.0f, 80.0f);
            _testCamera.CameraComponent.Target = new Vector3(0, 100f, 0);
            _testCamera.CameraComponent.ClearColor = new Vector4(1, 1, 1, 1);

            AddGameObject(_testCamera);
            SetCurrentCamera(_testCamera);

            AmbientColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);


            cube = new Cube();
            cube.Position = new Vector3(0, 100, 0);
            //cube.Rotation = new Vector3(0, 0, 10);
            cube.OnMouseIn += (a, b) => ((Cube)a).Material.Ambient = new Vector4(0.5f, 0.5f, 0.5f, 1);
            cube.OnMouseOut += (a, b) => ((Cube)a).Material.Ambient = new Vector4(1.0f, 0.5f, 0.5f, 1);

            //((Cube)cube).Material.Diffuse = new Vector4(0.5f, 0.5f, 0.5f, 1);
            //((Cube)cube).Material.Ambient = new Vector4(0.5f, 0.5f, 0.5f, 1);
            cube.Name = "testCube";
            AddGameObject(cube);


            base.Load();
        }



        private void AddBlocks(int line)
        {
            Random rand = new Random((int)DateTime.UtcNow.Ticks);

            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < _sizeX; i++)
                {
                    int r = rand.Next(0, 2);
                    int g = rand.Next(0, 2);
                    int b = rand.Next(0, 2);

                    var block = new Cube() { Scale = new Vector3(1, 1, 1) };

                    var z = (float)(Math.Cos(angle) * ((_sizeY / 2) - line));
                    var y = (float)(Math.Sin(angle) * ((_sizeY / 2) - line)) + (0.65f * (float)Math.Cos(angle));

                    block.Position = new Vector3(i - (_sizeX / 2 - 0.5f), y + j, z);

                    block.Rotation = new Vector3(-angle, 0.0f, 0.0f);
                    block.Material.Diffuse = new Vector4(r, g, b, 1);

                    block.AddComponent(new BoxColliderComponent());
                    block.AddComponent(new RigidBodyComponent());

                    AddGameObject(block);
                }
            }
        }

        private GameObject TestCompound(Vector3 position, Vector3 rotation)
        {
            var cubes = new List<GameObject>();

            for (int i = 0; i < 10; i++)
            {
                var cube = new Cube() { Scale = new Vector3(1.0f, 1.0f, 1.0f) };
                cube.Position = new Vector3(i, 0, 0);
                cube.AddComponent(new BoxColliderComponent());
                cube.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
                cube.Material.Diffuse = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                cubes.Add(cube);
            }

            var compoundObject = new CompoundObject(cubes, 50.0f);
            compoundObject.Position = position;
            compoundObject.Rotation = rotation;

            AddGameObject(compoundObject);

            return compoundObject;
        }
    }
}

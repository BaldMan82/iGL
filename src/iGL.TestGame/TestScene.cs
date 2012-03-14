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
        private CompoundObject _floor;
        private Camera _testCamera;
        private float alpha = 0;//(float)Math.PI * 2.0f; 
        private float angle = (float)Math.PI / 8.0f;
        private int _sizeX = 10;
        private int _sizeY = 10;

        public TestScene()
        {
            this.OnTick += new EventHandler<Engine.Events.TickEvent>(TestScene_OnTick);
        }

        void TestScene_OnTick(object sender, Engine.Events.TickEvent e)
        {
            //_testCamera.Position = new Vector3((float)(Math.Cos(alpha / 2.0f) * 30.0f), (float)(Math.Sin(alpha / 2.0f) * 10.0f) + 10.0f, (float)(Math.Sin(alpha / 2.0f) * 30.0f));

            //alpha += e.Elapsed * 1.0f;
        }

        public override void Load()
        {
            /* create camera */
            var properties = new PerspectiveProperties()
            {
                AspectRatio = MathHelper.DegreesToRadians(45.0f),
                FieldOfViewRadians = 3.0f / 2.0f,
                ZNear = 1.00f,
                ZFar = 1000.0f
            };

            _testCamera = new Camera(properties);
            _testCamera.Position = new Vector3(0.0f, 20.0f, 20.0f);
            _testCamera.CameraComponent.Target = new Vector3(0, 0f, 0);
            _testCamera.CameraComponent.ClearColor = new Vector4(1, 1, 1, 1);

            ShaderProgram.SetAmbientColor(new Vector4(1, 1, 1, 1));

            AddGameObject(_testCamera);
            SetCurrentCamera(_testCamera);

            ShaderProgram.SetAmbientColor(new Vector4(0.5f, 0.5f, 0.5f, 1.0f));          

            var sphere = new Sphere(0.5f);
            sphere.Position = new Vector3(0, 5, 0);
            sphere.AddComponent(new SphereColliderComponent());
            sphere.AddComponent(new RigidBodyComponent());

            AddGameObject(sphere);         
           
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

                    var block = new Cube(1, 1, 1);

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
                var cube = new Cube(1.0f, 1.0f, 1.0f);
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

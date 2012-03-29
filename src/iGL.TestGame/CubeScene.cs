using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.Math;

namespace iGL.TestGame
{
    public class CubeScene : Scene
    {
        private CompoundObject _floor;
        private PerspectiveCamera _testCamera;
        private float alpha = 0;//(float)Math.PI * 2.0f; 
        private float angle = (float)Math.PI / 8.0f;
        private int _sizeX = 10;
        private int _sizeY = 10;


        public CubeScene()
            : base(new Physics2d())
        {          
            this.OnTick += new EventHandler<Engine.Events.TickEvent>(TestScene_OnTick);
        }

        void TestScene_OnTick(object sender, Engine.Events.TickEvent e)
        {           
             _testCamera.Position = new Vector3((float)(Math.Cos(alpha / 2.0f) * 30.0f), /*(float)(Math.Sin(alpha / 2.0f)  10.0f) + */10.0f, (float)(Math.Sin(alpha / 2.0f) * 30.0f));

            alpha += e.Elapsed * 1.0f;
        }

        public override void Load()
        {
            /* create camera */

            _testCamera = new PerspectiveCamera();
           
            _testCamera.Position = new Vector3(0.0f, 20.0f, 20.0f);
            _testCamera.CameraComponent.Target = new Vector3(0, 0f, 0);

            AddGameObject(_testCamera);
            SetCurrentCamera(_testCamera);

            AmbientColor = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);

            /* add a point light */

            var pointlight = new PointLight();
            pointlight.Ambient = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            pointlight.Diffuse = new Vector4(1.0f, 1.0f, 0.8f, 1.0f);

            var lightObj = new GameObject();
            lightObj.Position = new Vector3(0, 10, 0);
            lightObj.AddComponent(new LightComponent(pointlight));

            AddGameObject(lightObj);
            SetCurrentLight(lightObj);


            var floor = new Cube() { Scale = new Vector3(_sizeY * 1000, 1.0f, _sizeX * 1000) };
            floor.Position = new Vector3(0.0f, -0.5f, 0.0f);
            floor.Material.Diffuse = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            floor.AddComponent(new BoxColliderComponent());
            floor.AddComponent(new RigidBodyComponent());

            AddGameObject(floor);

            Random rand = new Random();

            AddTimer(new Timer()
            {
                Action = () =>
                {
                    int x = rand.Next(-_sizeX/2, _sizeX/2);
                    int y = rand.Next(-_sizeY/2, _sizeY/2);

                    var cube = new Cube() { Scale = new Vector3(1, 1, 1) };
                    cube.Position = new Vector3(x+0.5f, 2.5f, y+0.5f);
                    cube.AddComponent(new BoxColliderComponent());
                    cube.AddComponent(new RigidBodyComponent());
                    cube.Material.Diffuse = new Vector4(1.0f, 0.0f, 0.0f, 1);
                    AddGameObject(cube);

                },
                Interval = TimeSpan.FromSeconds(0.1)
            });

           
        }
       


    }
}

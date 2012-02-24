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
        private float alpha = (float)Math.PI * 2.0f; 

        public TestScene()
        {
            this.TickEvent += new EventHandler<Engine.Events.TickEvent>(TestScene_OnTick);
        }

        void TestScene_OnTick(object sender, Engine.Events.TickEvent e)
        {
            _testCamera.Position = new Vector3((float)(Math.Cos(alpha / 2.0f) * 20.0f), (float)(Math.Sin(alpha / 2.0f) * 10.0f) + 10.0f, (float)(Math.Sin(alpha / 2.0f) * 20.0f));
          
            alpha += 0.02f;
        }
        
        public override void Load()
        {
            /* create camera */

            _testCamera = new Camera(MathHelper.DegreesToRadians(45.0f), 3.0f / 2.0f, 1.00f, 1000.0f);
            _testCamera.Position = new Vector3(0.0f, 10.0f, 20.0f);

            AddGameObject(_testCamera);
            SetCurrentCamera(_testCamera);

            ShaderProgram.SetAmbientColor(new Vector4(0.5f, 0.5f, 0.5f, 1.0f));
            
            /* add a point light */

            var pointlight = new PointLight();
            pointlight.Ambient = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);             
            pointlight.Diffuse = new Vector4(1.0f, 1.0f, 0.8f, 1.0f);
         
            /* add some test cubes */

            _testCube = new Cube(2.0f, 2.0f, 2.0f);
            _testCube.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
            _testCube.Material.Diffuse = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);
            _testCube.Position = new Vector3(0.0f, 60.0f, 0.0f);

            _testCube.AddComponent(new LightComponent(_testCube, pointlight));

            /* add box collider to cube */

            _testCube.AddComponent(new BoxColliderComponent(_testCube));
            var rigidBody = new RigidBodyComponent(_testCube);
            rigidBody.Mass = 5.0f;

            _testCube.AddComponent(rigidBody);

            AddGameObject(_testCube);

            SetCurrentLight(_testCube);                      

            var floor = new Cube(40.0f, 1.0f, 40.0f);
            floor.Position = new Vector3(0.0f, -1.0f, 0.0f);
            floor.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
            floor.Material.Diffuse = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            rigidBody = new RigidBodyComponent(floor);
            rigidBody.Mass = 0.0f;

            floor.AddComponent(new BoxColliderComponent(floor));
            floor.AddComponent(rigidBody);

            _testCube = floor;
            AddGameObject(floor);           

            for (int i = 0; i < 50; i++)
            {
                var cube = new Cube(1.0f, 1.0f, 1.0f);

                cube.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
                cube.Material.Diffuse = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
              
                cube.Position = new Vector3(0, 1.0f + i, i / 10.0f);

                cube.AddComponent(new BoxColliderComponent(cube));

                rigidBody = new RigidBodyComponent(cube);
                rigidBody.Mass = 5.0f;

                cube.AddComponent(rigidBody);

                AddGameObject(cube);
            }                           
        }
    }
}

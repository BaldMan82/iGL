using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using OpenTK;

namespace iGL.TestGame
{
    public class TestScene : Scene
    {
        private Cube _testCube;
        private Camera _testCamera;
        private LightObject _testLight;
        private float alpha; 

        public TestScene()
        {
            this.TickEvent += new EventHandler<Engine.Events.TickEvent>(TestScene_OnTick);
        }

        void TestScene_OnTick(object sender, Engine.Events.TickEvent e)
        {
            _testCamera.Position = new Vector3((float)(Math.Cos(alpha/5.0f) * 20.0f), 10.0f, (float)(Math.Sin(alpha/5.0f) * 20.0f));
            //_testLight.Position = new Vector3((float)(Math.Cos(Math.PI - alpha*2.0f) * 20.0f), 5.0f, (float)(Math.Sin(Math.PI - alpha*2.0f) * 20.0f));

            _testCube.Rotation += new Vector3(0.1f, 0.0f, 0.0f);

            alpha += 0.02f;
        }
        
        public override void Load()
        {
            /* create camera */

            _testCamera = new Camera(MathHelper.DegreesToRadians(45.0f), 3.0f / 2.0f, 1.00f, 1000.0f);
            //_testCamera.Position = new Vector3(0.0f, 10.0f, 10.0f);

            AddGameObject(_testCamera);
            SetCurrentCamera(_testCamera);

            ShaderProgram.SetAmbientColor(new Vector4(0.5f, 0.5f, 0.5f, 1.0f));
            
            /* add a point light */

            var pointlight = new PointLight();
            pointlight.Ambient = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);             
            pointlight.Diffuse = new Vector4(1.0f, 1.0f, 0.8f, 1.0f);             
            
            _testLight = new LightObject(pointlight);
            _testLight.Position = new Vector3(-40, 20, 10);

            AddGameObject(_testLight);

            SetCurrentLight(_testLight);

            /* add some test cubes */

            _testCube = new Cube(1.0f, 1.0f, 1.0f);
            _testCube.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
            _testCube.Material.Diffuse = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);     
            _testCube.Position = new Vector3(0.0f, 10.0f, 0.0f);

            /* add box collider to cube */

            _testCube.AddComponent(new BoxColliderComponent(_testCube));
            var rigidBody = new RigidBodyComponent(_testCube);
            rigidBody.Mass = 10.0f;

            _testCube.AddComponent(rigidBody);

            AddGameObject(_testCube);
            
            /* let the camera follow the testcube */
            //_testCamera.CameraComponent.Target = _testCube.Position;

            var floor = new Cube(20.0f, 1.0f, 20.0f);
            floor.Position = new Vector3(0.0f, -1.0f, 0.0f);
            floor.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0.0f);
            floor.Material.Diffuse = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            rigidBody = new RigidBodyComponent(floor);
            rigidBody.Mass = 0.0f;

            floor.AddComponent(new BoxColliderComponent(floor));
            floor.AddComponent(rigidBody);

            AddGameObject(floor);

            //for (int i = 0; i < 10; i++)
            //{
            //    var cube = new Cube(1.0f, 1.0f, 1.0f);
            //    cube.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0);
            //    cube.Material.Diffuse = new Vector4(i / 10.0f, i / 10.0f, i / 10.0f, 0);
                
            //    cube.Position = new Vector3(4.5f, 0.0f, -4.5f + i);

            //    AddGameObject(cube);
            //}                     

            for (int i = 0; i < 50; i++)
            {
                var cube = new Cube(1.0f, 1.0f, 1.0f);
                cube.Material.Ambient = new Vector4(0.1f, 0.1f, 0.1f, 0);
                cube.Material.Diffuse = new Vector4(i / 50.0f, i / 50.0f, i / 50.0f, 0);

                cube.Position = new Vector3(-5, 5.0f+i, i / 20.0f);

                cube.AddComponent(new BoxColliderComponent(cube));
                
                rigidBody = new RigidBodyComponent(cube);
                rigidBody.Mass = 1.0f;
                
                cube.AddComponent(rigidBody);

                AddGameObject(cube);
            }                           
        }
    }
}

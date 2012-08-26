using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using System.Xml.Linq;
using iGL.Engine.Math;

namespace iGL.TestGame.GameObjects
{
    [RequiredChild(typeof(Cube), SphereMagnet.SelectionCubeId)]
    [RequiredChild(typeof(MagnetHalo), SphereMagnet.HaloPlaneId)]
    public class SphereMagnet : RigidFarseerSphere
    {
        public SphereMagnet(XElement element) : base(element) { }

        public SphereMagnet() { }
          
        private RigidBodyFarseerComponent _rigidBodyComponent;
        private MeshComponent _meshComponent;
        private Cube _selectionCube;
        private MagnetHalo _haloPlane;

        private float _initialMass;
        private const string SelectionCubeId = "16af2307-ce70-453b-a8ab-54bad0d51524";
        private const string HaloPlaneId = "42bf2307-de70-453b-c8ab-54bad0d51524"; 

        protected override void Init()
        {
            base.Init();

            _rigidBodyComponent = this.Components.Single(c => c is RigidBodyFarseerComponent) as RigidBodyFarseerComponent;
            _meshComponent = this.Components.Single(c => c is MeshComponent) as MeshComponent;

            _rigidBodyComponent.IsGravitySource = true;
            _rigidBodyComponent.GravityRange = 1.0f;
            _rigidBodyComponent.IsStatic = true;
            _rigidBodyComponent.Mass = 1000;

            _selectionCube = this.Children.Single(c => c.Id == SelectionCubeId) as Cube;            
            _selectionCube.OnMouseDown += SphereMagnet_OnMouseDown;
            _selectionCube.Visible = false;

            _haloPlane = this.Children.Single(c => c.Id == HaloPlaneId) as MagnetHalo;
            _haloPlane.Material.TextureName = "MagnetHalo";
            
            
            this.OnScale += new EventHandler<Engine.Events.ScaleEvent>(SphereMagnet_OnScale);
        }

        void SphereMagnet_OnScale(object sender, Engine.Events.ScaleEvent e)
        {
            _selectionCube.Scale = new Vector3(2f, 2f, 5.0f);
            _selectionCube.Visible = false;

            _haloPlane.Scale = new Vector3(Scale.X + _rigidBodyComponent.GravityRange, Scale.Y + _rigidBodyComponent.GravityRange, 1);
        }       

        public override void Load()
        {
            base.Load();

            if (Game.InDesignMode) return;

            _initialMass = _rigidBodyComponent.Mass;
            _haloPlane.Visible = false;
            _rigidBodyComponent.Mass = 0f;
        }

        void SphereMagnet_OnMouseDown(object sender, Engine.Events.MouseButtonDownEvent e)
        {
            if (Game.InDesignMode) return;

            if (_rigidBodyComponent.Mass == 0)
            {
                _rigidBodyComponent.Mass = _initialMass;
                _meshComponent.Material.Ambient = new Vector4(0, 0, 1, 1);
                _haloPlane.Visible = true;
            }
            else
            {
                _rigidBodyComponent.Mass = 0;
                _meshComponent.Material.Ambient = new Vector4(0, 0, 0, 1);
                _haloPlane.Visible = false;
            }
        }
      
    }
}

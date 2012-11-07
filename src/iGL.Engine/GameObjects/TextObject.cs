using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;
using System.Xml.Linq;
using iGL.Engine.Resources;
using iGL.Engine.BitmapFont;

namespace iGL.Engine
{
    [Serializable]
    [RequiredComponent(typeof(MeshComponent), TextObject.MeshComponentId)]
    [RequiredComponent(typeof(MeshRenderComponent), TextObject.MeshRenderComponentId)]
    [RequiredComponent(typeof(TextComponent), TextObject.TextComponentId)]
    public class TextObject : GameObject
    {       
        public Material Material
        {
            get
            {
                return _meshComponent.Material;
            }
        }

        protected MeshComponent _meshComponent;
        protected MeshRenderComponent _meshRenderComponent;
        protected TextComponent _textComponent;

        private const string MeshComponentId = "f2dae056-2ff7-443f-aed1-0afd3db7b0bf";
        private const string MeshRenderComponentId = "16af2307-be79-453a-a8ab-54bad0d31525";
        private const string TextComponentId = "32dae056-2ff7-643f-aed1-0afd3db7b0bf";

        public TextObject(XElement element) : base(element) { }

        public TextObject() { }

        protected override void Init()
        {
            _meshComponent = Components.Single(c => c.Id == MeshComponentId) as MeshComponent;
            _meshRenderComponent = Components.Single(c => c.Id == MeshRenderComponentId) as MeshRenderComponent;
            _textComponent = Components.Single(c => c.Id == TextComponentId) as TextComponent;
            _textComponent.FontName = "bmpfont";
            _meshComponent.Material.TextureName = "bmpfont_0.text";

            DistanceSorting = true;
        }

        public void SetText(string text)
        {
            _textComponent.Text = text;
            _textComponent.Reload();
        }
    }
}

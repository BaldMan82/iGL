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
    public class TextObject : GameObject
    {
        public string FontName { get; set; }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                UpdateText();
            }
        }

        public Material Material
        {
            get
            {
                return _meshComponent.Material;
            }
        }

        private MeshComponent _meshComponent;
        private MeshRenderComponent _meshRenderComponent;

        private const string MeshComponentId = "f2dae056-2ff7-443f-aed1-0afd3db7b0bf";
        private const string MeshRenderComponentId = "16af2307-be79-453a-a8ab-54bad0d31525";

        private Font _font;
        private string _text;

        public TextObject(XElement element) : base(element) { }

        public TextObject() { }

        protected override void Init()
        {
            _meshComponent = Components.Single(c => c.Id == MeshComponentId) as MeshComponent;
            _meshRenderComponent = Components.Single(c => c.Id == MeshRenderComponentId) as MeshRenderComponent;

            FontName = "bmp";
        }

        private void UpdateText()
        {
            if (!IsLoaded) return;

            var text = Text == null ? string.Empty : Text;

            _font = Scene.Resources.FirstOrDefault(r => r.Name == FontName) as Font;
            if (_font == null) return;

            var texture = Scene.Resources.FirstOrDefault(r => r.Name == _font.TextureName) as Texture;
            if (texture == null) return;

            var vertices = new Vector3[text.Length * 6];
            var indices = new short[text.Length * 6];
            var uv = new Vector2[text.Length * 6];

            float cursor = 0;

            for (int i = 0; i < text.Length; i++)
            {
                Character data;
                int kerning;

                data = _font.BmpFont[text[i]];
                char lastChar = i > 0 ? text[i - 1] : ' ';

                kerning = _font.BmpFont.GetKerning(lastChar, text[i]);

                float width = (data.Bounds.Right - data.Bounds.Left) / 100.0f;
                float height = (data.Bounds.Bottom - data.Bounds.Top) / 100.0f;
                float offsetX = data.Offset.X / 100.0f;
                float offsetY = data.Offset.Y / 100.0f;

                cursor += kerning + offsetX;

                // front (+y)                                                
                vertices[0 + i * 6] = new Vector3(width + cursor, -height - offsetY, 0);
                vertices[1 + i * 6] = new Vector3(0 + cursor, -offsetY, 0);
                vertices[2 + i * 6] = new Vector3(0 + cursor, -height - offsetY, 0);
                vertices[3 + i * 6] = new Vector3(width + cursor, -height - offsetY, 0);
                vertices[4 + i * 6] = new Vector3(width + cursor, -offsetY, 0);
                vertices[5 + i * 6] = new Vector3(0 + cursor, -offsetY, 0);

                indices[0 + i * 6] = (short)(0 + i * 6);
                indices[1 + i * 6] = (short)(1 + i * 6);
                indices[2 + i * 6] = (short)(2 + i * 6);
                indices[3 + i * 6] = (short)(3 + i * 6);
                indices[4 + i * 6] = (short)(4 + i * 6);
                indices[5 + i * 6] = (short)(5 + i * 6);

                float uvLeft = data.Bounds.Left * 1.0f / (texture.Width * 1.0f);
                float uvRight = data.Bounds.Right * 1.0f / (texture.Width * 1.0f);
                float uvBottom = data.Bounds.Top * 1.0f / (texture.Height * 1.0f);
                float uvTop = data.Bounds.Bottom * 1.0f / (texture.Height * 1.0f);

                uv[0 + i * 6] = new Vector2(uvRight, uvTop);
                uv[1 + i * 6] = new Vector2(uvLeft, uvBottom);
                uv[2 + i * 6] = new Vector2(uvLeft, uvTop);
                uv[3 + i * 6] = new Vector2(uvRight, uvTop);
                uv[4 + i * 6] = new Vector2(uvRight, uvBottom);
                uv[5 + i * 6] = new Vector2(uvLeft, uvBottom);

                cursor += (float)data.XAdvance / 100.0f;
            }

            /* calculate extensions and adjust to center */

            Vector3 vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (var vertex in vertices)
            {
                if (vertex.X < vMin.X) vMin.X = vertex.X;
                if (vertex.X > vMax.X) vMax.X = vertex.X;

                if (vertex.Y < vMin.Y) vMin.Y = vertex.Y;
                if (vertex.Y > vMax.Y) vMax.Y = vertex.Y;

                if (vertex.Z < vMin.Z) vMin.Z = vertex.Z;
                if (vertex.Z > vMax.Z) vMax.Z = vertex.Z;
            }

            float halfWidth = (vMax - vMin).X / 2.0f;
            float halfHeight = (vMax - vMin).Y / 2.0f;

            for (int i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                vertices[i] = new Vector3(vertex.X - halfWidth, vertex.Y + halfHeight, 0);
            }

            _meshComponent.Vertices = vertices;
            _meshComponent.Indices = indices;
            _meshComponent.UV = uv;
            _meshComponent.Material.TextureName = _font.TextureName;

            _meshComponent.CalculateNormals();
          
            _meshRenderComponent.Reload();
        }


        public override void Load()
        {
            base.Load();

            UpdateText();
        }

        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);            
        }
    }
}

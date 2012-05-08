using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;
using System.Runtime.Serialization;
using iGL.Engine.Resources;
using iGL.Engine.Math;
using iGL.Engine.BitmapFont;


namespace iGL.Engine
{
    [Serializable]
    public class TextComponent : GameComponent
    {
        public string FontName { get; set; }

        public string Text { get; set; }

        public int LineLength { get; set; }

        public override bool InternalLoad()
        {          
            return LoadText();
        }

        public TextComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public TextComponent() { }

        protected override void Init()
        {
            base.Init();

            LineLength = 50;
        } 

        public override void Tick(float timeElapsed)
        {            
        }

        private Font _font;       

        private bool LoadText()
        {
            if (GameObject == null || GameObject.Scene == null) return false;

            var text = Text == null ? string.Empty : Text;

            _font = GameObject.Scene.Resources.FirstOrDefault(r => r.Name == FontName) as Font;
            if (_font == null) return false;

            var texture = GameObject.Scene.Resources.FirstOrDefault(r => r.Name == _font.TextureName) as Texture;
            if (texture == null) return false;

            var meshComponent = GameObject.Components.FirstOrDefault(c => c is MeshComponent) as MeshComponent;
            if (meshComponent == null) return false;

            var meshRenderComponent = GameObject.Components.FirstOrDefault(c => c is MeshRenderComponent) as MeshRenderComponent;
            if (meshRenderComponent == null) return false;

            var vertices = new Vector3[text.Length * 6];
            var indices = new short[text.Length * 6];
            var uv = new Vector2[text.Length * 6];

            float cursor = 0;
            float cursorY = 0;
            int lineCount = 0;

            var words = text.Split(' ');
            int j = 0;

            for (int w = 0; w < words.Length;w++)
            {
                var word = words[w];
                if (w < words.Length - 1) word += ' ';

                if (word.Length + lineCount > LineLength)
                {
                    cursor = 0;
                    cursorY += 0.25f;
                    lineCount = 0;                   
                }

                for (int i = 0; i < word.Length; i++)
                {
                    Character data;
                    int kerning;

                    data = _font.BmpFont[word[i]];
                    char lastChar = i > 0 ? word[i - 1] : ' ';

                    kerning = _font.BmpFont.GetKerning(lastChar, word[i]);

                    float width = (data.Bounds.Right - data.Bounds.Left) / 100.0f;
                    float height = (data.Bounds.Bottom - data.Bounds.Top) / 100.0f;
                    float offsetX = data.Offset.X / 100.0f;
                    float offsetY = data.Offset.Y / 100.0f;

                    lineCount++;                    

                    cursor += kerning + offsetX;

                    // front (+y)                                                
                    vertices[0 + j * 6] = new Vector3(width + cursor, -height - offsetY - cursorY, 0);
                    vertices[1 + j * 6] = new Vector3(0 + cursor, -offsetY - cursorY, 0);
                    vertices[2 + j * 6] = new Vector3(0 + cursor, -height - offsetY - cursorY, 0);
                    vertices[3 + j * 6] = new Vector3(width + cursor, -height - offsetY - cursorY, 0);
                    vertices[4 + j * 6] = new Vector3(width + cursor, -offsetY - cursorY, 0);
                    vertices[5 + j * 6] = new Vector3(0 + cursor, -offsetY - cursorY, 0);

                    indices[0 + j * 6] = (short)(0 + j * 6);
                    indices[1 + j * 6] = (short)(1 + j * 6);
                    indices[2 + j * 6] = (short)(2 + j * 6);
                    indices[3 + j * 6] = (short)(3 + j * 6);
                    indices[4 + j * 6] = (short)(4 + j * 6);
                    indices[5 + j * 6] = (short)(5 + j * 6);

                    float uvLeft = data.Bounds.Left * 1.0f / (texture.Width * 1.0f);
                    float uvRight = data.Bounds.Right * 1.0f / (texture.Width * 1.0f);
                    float uvBottom = data.Bounds.Top * 1.0f / (texture.Height * 1.0f);
                    float uvTop = data.Bounds.Bottom * 1.0f / (texture.Height * 1.0f);

                    uv[0 + j * 6] = new Vector2(uvRight, uvTop);
                    uv[1 + j * 6] = new Vector2(uvLeft, uvBottom);
                    uv[2 + j * 6] = new Vector2(uvLeft, uvTop);
                    uv[3 + j * 6] = new Vector2(uvRight, uvTop);
                    uv[4 + j * 6] = new Vector2(uvRight, uvBottom);
                    uv[5 + j * 6] = new Vector2(uvLeft, uvBottom);

                    cursor += (float)data.XAdvance / 100.0f;
                    j++;
                }
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

            meshComponent.Vertices = vertices;
            meshComponent.Indices = indices;
            meshComponent.UV = uv;
            meshComponent.Material.TextureName = _font.TextureName;
            meshComponent.RefreshTexture();

            meshComponent.CalculateNormals();

            if (GameObject.IsLoaded) meshRenderComponent.Reload();

            return true;
        }

        public void Reload()
        {
            InternalLoad();
        }
    }
}

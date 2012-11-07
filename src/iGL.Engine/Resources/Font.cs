using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace iGL.Engine.Resources
{
    public class Font : Resource
    {
        public BitmapFont.BitmapFont BmpFont { get; private set; }
        public string TextureName { get; private set; }

        protected override bool InternalLoad()
        {
            if (!Resource.AssemblyResources.Contains(base.ResourceName)) return false;
       
            using (var stream = Resource.ResourceAssembly.GetManifestResourceStream(base.ResourceName))
            {
                var textReader = new StreamReader(stream);
                var font = BitmapFont.BitmapFontLoader.LoadFontFromTextReader(textReader);

                /* load textures */
                if (font.Pages.Length > 1) throw new NotSupportedException("Only one page font supported");

                var page = font.Pages.First();

                TextureName = string.Format("{0}.text", page.FileName.Split(new char[] { '.' })[0]);
                    
                /* find resource */
                var resourceName = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetManifestResourceNames()).FirstOrDefault(name => name.Contains(TextureName));
                if (resourceName == null) throw new FileNotFoundException(resourceName + " missing.");

                if (!Scene.Resources.Any(r => r is Texture && r.ResourceName == resourceName))
                {
                    var texture = new Texture() { ResourceName = resourceName, Name = TextureName };
                    Scene.AddResource(texture);
                }
                
                BmpFont = font;                
            }

            return true;
        }
    }
}

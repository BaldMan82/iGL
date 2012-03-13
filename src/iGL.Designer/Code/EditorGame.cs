using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.GL;

namespace iGL.Designer.Code
{
    public class EditorGame : Game
    {
        public EditorGame(IGL gl) : base(gl) { }       

        public override void Load()
        {
            base.Load();           
            LoadScene();
        }
    }
}

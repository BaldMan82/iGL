using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.GL;

namespace iGL.Designer
{
    public class EditorGame : Game
    {
        private static EditorGame _game;

        private EditorGame() : base(new WinGL())
        {

        }

        public static EditorGame Instance()
        {
            lock (typeof(EditorGame))
            {
                if (_game == null)
                {
                    _game = new EditorGame();
                }
            }

            return _game;
        }          

        public override void Load()
        {
            base.Load();                       
        }
    }
}

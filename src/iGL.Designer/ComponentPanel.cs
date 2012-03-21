using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iGL.Engine;
using System.Reflection;

namespace iGL.Designer
{
    public partial class ComponentPanel : UserControl
    {
        public GameComponent GameComponent { get; private set; }

        private static Dictionary<Type, Type> _gameObjectDialogTypes;

        public ComponentPanel()
        {
            InitializeComponent();

            if (_gameObjectDialogTypes == null)
            {
                _gameObjectDialogTypes = new Dictionary<Type, Type>();

                /* find all dialog types */
                var asm = Assembly.GetExecutingAssembly();

                var gameObjectDialogs = asm.GetTypes().Where(t => t.GetCustomAttributes(false).Any(o => o.GetType() == typeof(GameObjectDialogAttribute))).ToList();
                _gameObjectDialogTypes = new Dictionary<Type, Type>();

                foreach (var gameObjectDlg in gameObjectDialogs)
                {
                    var attribute = gameObjectDlg.GetCustomAttributes(false).First(o => o.GetType() == typeof(GameObjectDialogAttribute)) as GameObjectDialogAttribute;
                    _gameObjectDialogTypes.Add(attribute.GameObjectType, gameObjectDlg);

                }
            }
        }

        public void LoadComponent(GameComponent component)
        {
            GameComponent = component;

            var componentControl = Activator.CreateInstance(_gameObjectDialogTypes[component.GetType()]) as ComponentControl;
            componentControl.Component = component;

            lblComponentName.Text = component.GetType().Name;
            
            contentPanel.Controls.Add(componentControl);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var dlg = new ComponentSelectDlg();           

            dlg.ShowDialog(this);

            if (dlg.SelectedComponentType != null)
            {
                var component = Activator.CreateInstance(dlg.SelectedComponentType) as GameComponent;
                GameComponent.GameObject.AddComponent(component);          
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            GameComponent.GameObject.RemoveComponent(GameComponent);
        }
    }
}

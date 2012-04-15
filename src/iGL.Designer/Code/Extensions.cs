using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using iGL.Engine.Math;
using System.Windows.Forms;
using System.Globalization;
using iGL.Engine.Events;

namespace iGL.Designer
{
    public static class Extensions
    {
        public static string ToInvariantText(this float val)
        {
            return val.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }      

        public static Color ToSystemColor(this Vector4 vec)
        {
            return Color.FromArgb((int)(vec.X * 255f), (int)(vec.Y * 255f), (int)(vec.Z * 255f));
        }

        public static Vector4 ToVectorColor(this Color col)
        {
            return new Vector4(col.R / 255.0f, col.G / 255.0f, col.B / 255.0f, 1);
        }

        public static MouseButton ToMouseButton(this MouseButtons buttons)
        {
            if (buttons == MouseButtons.Left) return MouseButton.Button1;
            if (buttons == MouseButtons.Right) return MouseButton.Button2;
            if (buttons == MouseButtons.Middle) return MouseButton.ButtonMiddle;

            return MouseButton.Button1;
        }      

        public static void HookAllChangeEvents(this System.Windows.Forms.Control.ControlCollection controls, Action action)
        {
            /* hook up all textboxes for change events */
            foreach (var control in controls)
            {
                if (control is TextBox)
                {
                    var t = control as TextBox;
                    t.TextChanged += (a, b) => action();
                }
                else if (control is RadioButton)
                {
                    var r = control as RadioButton;
                    r.CheckedChanged += (a, b) => action();
                }
                else if (control is CheckBox)
                {
                    var c = control as CheckBox;
                    c.CheckedChanged += (a, b) => action();
                }
                else
                {
                    var c = control as Control;
                    c.Controls.HookAllChangeEvents(action);
                }
                
            }
           
        }

        public static float TextToFloat(this TextBox t, float defaultValue = 0.0f)
        {
            if (string.IsNullOrEmpty(t.Text) ||  t.Text == "-") return defaultValue;

            float result;
            if (float.TryParse(t.Text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign , CultureInfo.InvariantCulture, out result))
            {
                return result;
            }
            else
            {
                MessageBox.Show("Invalid float value.");
                t.Text = defaultValue.ToInvariantText();
            }

            return defaultValue;
            
        }

        public static int TextToInt(this TextBox t, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(t.Text) || t.Text == "-") return defaultValue;

            int result;
            if (int.TryParse(t.Text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }
            else
            {
                MessageBox.Show("Invalid int value.");
                t.Text = defaultValue.ToString();
            }

            return defaultValue;

        }
    }
}

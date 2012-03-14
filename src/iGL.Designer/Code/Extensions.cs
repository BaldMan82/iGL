using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using iGL.Engine.Math;
using System.Windows.Forms;
using System.Globalization;

namespace iGL.Designer
{
    public static class Extensions
    {
        public static string ToInvariant(this float val)
        {
            return val.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static Color ToSystemColor(this Vector4 vec)
        {
            return Color.FromArgb((int)(vec.X * 255f), (int)(vec.Y * 255f), (int)(vec.Z * 255f));
        }

        public static Vector4 ToVectorColor(this Color col)
        {
            return new Vector4(col.R / 255.0f, col.G / 255.0f, col.B / 255.0f, 0);
        }

        public static float TextToFloat(this TextBox t)
        {
            if (string.IsNullOrEmpty(t.Text) ||  t.Text == "-") return 0f;

            float result;
            if (float.TryParse(t.Text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign , CultureInfo.InvariantCulture, out result))
            {
                return result;
            }
            else
            {
                MessageBox.Show("Invalid float value.");
                t.Text = "0";
            }

            return 0f;
            
        }
    }
}

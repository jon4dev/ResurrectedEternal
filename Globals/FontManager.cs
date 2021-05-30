using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResurrectedEternal.Globals
{
    public class FontManager
    {
        private System.Drawing.Text.InstalledFontCollection _fontCollection = new System.Drawing.Text.InstalledFontCollection();

        public string[] Fonts;

        public FontManager()
        {
            Fonts = new string[_fontCollection.Families.Length];
            for (int i = 0; i < Fonts.Length; i++)
            {
                Fonts[i] = _fontCollection.Families[i].Name;
            }
        }


    }
}

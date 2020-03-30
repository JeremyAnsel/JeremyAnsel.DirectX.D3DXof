using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XMaterial
    {
        public string Name { get; set; }

        public bool IsReference { get; set; }

        public XColorRgba FaceColor { get; set; }

        public float Power { get; set; }

        public XColorRgb SpecularColor { get; set; }

        public XColorRgb EmissiveColor { get; set; }

        public string Filename { get; set; }

        public XEffectInstance EffectInstance { get; set; }

        public override string ToString()
        {
            if (this.IsReference)
            {
                return string.Format(CultureInfo.InvariantCulture, "Ref {0}", this.Name);
            }
            else
            {
                return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", this.Name, this.FaceColor, this.Filename);
            }
        }
    }
}

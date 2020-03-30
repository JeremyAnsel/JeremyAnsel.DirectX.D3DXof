using System;
using System.Collections.Generic;
using System.Text;

namespace JeremyAnsel.DirectX.D3DXof
{
    public sealed class XEffectInstance
    {
        public string Name { get; set; }

        public string EffectFilename { get; set; }

        public List<Tuple<string, int>> IntegerParameters { get; } = new List<Tuple<string, int>>();

        public List<Tuple<string, float[]>> FloatParameters { get; } = new List<Tuple<string, float[]>>();

        public List<Tuple<string, string>> StringParameters { get; } = new List<Tuple<string, string>>();
    }
}

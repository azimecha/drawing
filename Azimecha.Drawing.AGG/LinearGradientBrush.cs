using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.AGG {

    public class LinearGradientBrush : Brush, ILinearGradientBrush {
        private IDictionary<float, Color> _dicStops = new SortedDictionary<float, Color>();

        public LinearGradientBrush(IEnumerable<KeyValuePair<float, Color>> enuStops, float fAngle) {
            List<Interop.AwGradientStop> lstStops = new List<Interop.AwGradientStop>();

            foreach (KeyValuePair<float, Color> kvp in enuStops) {
                lstStops.Add(new Interop.AwGradientStop() {
                    clr = new Interop.AwColor() { r = kvp.Value.R, g = kvp.Value.G, b = kvp.Value.B, a = kvp.Value.A },
                    pos = kvp.Key
                });

                _dicStops.Add(kvp);
            }

            Interop.AwGradientStop[] arrStops = lstStops.ToArray();

            Handle.TakeObject(Interop.Functions.Loader.GetMethod<Interop.Functions.AwCreateLinearGradientBrush>()(arrStops, arrStops.Length, fAngle), true);
            if (!Handle.IsHandleValid)
                throw new ObjectCreationFailedException($"Error creating linear gradient brush with {arrStops.Length} stops");

            Angle = fAngle;
        }


        public float Angle { get; private set; }

        public IEnumerable<KeyValuePair<float, Color>> GradientStops => _dicStops;
    }
}

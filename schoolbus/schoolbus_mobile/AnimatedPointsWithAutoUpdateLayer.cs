using Mapsui;
using Mapsui.Layers;
using Mapsui.Layers.AnimatedLayers;
using Mapsui.Providers;
using Mapsui.Styles;

namespace schoolbus_mobile
{
    public class AnimatedPointsWithAutoUpdateLayer : AnimatedPointLayer
    {
        public AnimatedPointsWithAutoUpdateLayer()
            : base(new DynamicMemoryProvider())
        {
            Style = new SymbolStyle { Fill = { Color = new Mapsui.Styles.Color(255, 215, 0, 200) }, SymbolScale = 0.9 };
        }

        private class DynamicMemoryProvider : MemoryProvider
        {
            public override async Task<IEnumerable<IFeature>> GetFeaturesAsync(FetchInfo fetchInfo)
            {
                var features = new List<IFeature>();
                return features;
            }
        }
    }
}
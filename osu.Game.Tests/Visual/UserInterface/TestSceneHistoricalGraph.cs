using System;
using System.Collections.Generic;
using osu.Framework.MathUtils;
using osu.Game.Graphics.UserInterface;
using osu.Framework.Graphics;

namespace osu.Game.Tests.Visual.UserInterface
{
    public class TestSceneHistoricalGraph : OsuTestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(HistoricalGraph),
        };

        private readonly HistoricalGraph graph;

        public TestSceneHistoricalGraph()
        {
            Add(graph = new HistoricalGraph
            {
                Width = 300,
                Height = 180,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            });

            AddStep("Create values", () =>
            {
                var values = new List<KeyValuePair<DateTimeOffset, float>>();

                for (int i = 0; i < 5; i++)
                {
                    values.Add(new KeyValuePair<DateTimeOffset, float>(DateTimeOffset.Now.AddDays(-i), RNG.NextSingle() * 100));
                }

                graph.Values = values;
            });
        }
    }
}

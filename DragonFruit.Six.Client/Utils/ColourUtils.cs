// Dragon6 Client Copyright (c) DragonFruit Network <inbox@dragonfruit.network>
// Licensed under GNU AGPLv3. Refer to the LICENSE file for more info

using System;
using System.Collections.Generic;
using System.Drawing;

namespace DragonFruit.Six.Client.Utils
{
    /// <remarks>Logic based on code from https://github.com/ppy/osu/blob/bff82a71a97c443cfcc4099a99e5ee19cd55abc5/osu.Game/Utils/ColourUtils.cs</remarks>
    public static class ColourUtils
    {
        /// <summary>
        /// Samples from a given linear gradient at a certain specified point.
        /// </summary>
        /// <param name="gradient">The gradient, defining the colour stops and their positions (in [0-1] range) in the gradient.</param>
        /// <param name="point">The point to sample the colour at.</param>
        /// <returns>A <see cref="Color"/> sampled from the linear gradient.</returns>
        public static string SampleFromGradient(IReadOnlyList<(float position, int colour)> gradient, float point)
        {
            if (point < gradient[0].position)
                return $"#{gradient[0].colour:X}";

            for (int i = 0; i < gradient.Count - 1; i++)
            {
                var startStop = gradient[i];
                var endStop = gradient[i + 1];

                if (point >= endStop.position)
                    continue;

                var interpolated = InterpolateColour(point, Color.FromArgb(startStop.colour), Color.FromArgb(endStop.colour), startStop.position, endStop.position);
                return $"#{interpolated.ToArgb():X}";
            }

            return $"#{gradient[^1].colour:X}";
        }

        private static Color InterpolateColour(double value, Color startColour, Color endColour, double startValue, double endValue)
        {
            if (startColour == endColour)
                return startColour;

            var current = value - startValue;
            var duration = endValue - startValue;

            if (duration == 0 || current == 0)
                return startColour;

            var t = (float)Math.Max(0, Math.Min(1, current / duration));

            return Color.FromArgb(
                (int)(startColour.A + t * (endColour.A - startColour.A)),
                (int)(startColour.R + t * (endColour.R - startColour.R)),
                (int)(startColour.G + t * (endColour.G - startColour.G)),
                (int)(startColour.B + t * (endColour.B - startColour.B)));
        }
    }
}

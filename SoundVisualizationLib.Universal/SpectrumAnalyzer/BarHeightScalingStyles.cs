using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundVisualizationLib.Universal
{

    /// <summary>
    /// The different ways that the bar height can be scaled by the spectrum analyzer.
    /// </summary>
    public enum BarHeightScalingStyles
    {
        /// <summary>
        /// A decibel scale. Formula: 20 * Log10(FFTValue).
        /// Total bar height is scaled from -90 to 0 dB.
        /// </summary>
        Decibel,

        /// <summary>
        /// A non-linear squareroot scale. Formula: Sqrt(FFTValue) * 2 * BarHeight.
        /// </summary>
        Sqrt,

        /// <summary>
        /// A linear scale. Formula: 9 * FFTValue * BarHeight.
        /// </summary>
        Linear
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundVisualizationLib.Universal
{
    /// <summary>
    /// Provides access to sound player functionality needed
    /// to render a spectrun analyzer.
    /// </summary>
    public interface ISpectrumPlayer:ISoundPlayer
    {
        /// <summary>
        /// Assigns current FFT data to a buffer.
        /// </summary>
        /// <param name="fftDataBuffer">The buffer to copy FFT data.</param>
        /// <returns>True id data was written to the buffer, otherwise false.</returns>
        bool GetFFTData(float[] fftDataBuffer);

        /// <summary>
        /// Gets the index in the FFT data buffer for a given frequency.
        /// </summary>
        /// <param name="frequency">The frequency for which to obtain a buffer index</param>
        /// <returns>An index in the FFT data buffer</returns>
        int GetFFTFrequencyIndex(int frequency);
    }
}

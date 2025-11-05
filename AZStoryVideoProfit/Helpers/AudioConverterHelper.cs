using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Helpers
{
    public class AudioConverterHelper
    {
        // Define the WAV format parameters
        private const int Channels = 1;
        private const int SampleRate = 24000;
        private const int BitsPerSample = 16; // Corresponds to 2 bytes (2 * 8 = 16)
        private const int BytesPerSample = BitsPerSample / 8;

        public static void ProcessAudioChunks(List<string> arrBase64, string output_path)
        {
            // 1. Decode Base64 audio data
            List<byte[]> allAudioData = new List<byte[]>();


            foreach (string base64Item in arrBase64) {

                allAudioData.Add(Convert.FromBase64String(base64Item));
            }

            

            // 2. Logging            

            // 3. Combine and Write to WAV file using NAudio
            try
            {
                // Set up the WaveFormat
                var waveFormat = new WaveFormat(SampleRate, BitsPerSample, Channels);

                // Use the WaveFileWriter to create the WAV file
                using (var writer = new WaveFileWriter(output_path, waveFormat))
                {
                    // Write each decoded byte array chunk to the WAV file
                    foreach (byte[] audioData in allAudioData)
                    {
                        writer.Write(audioData, 0, audioData.Length);
                    }

                    // NAudio's WaveFileWriter handles the header generation and finalization upon disposal (using statement).
                }
                
            }
            catch (Exception ex)
            {
                
                //throw; // Re-throw the exception or handle it as appropriate
            }
        }
    }
}

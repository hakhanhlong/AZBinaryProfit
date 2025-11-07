using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace AZStoryVideoProfit.Helpers
{
    public class AudioMixerHelper
    {
        // Hàm Log Information giả định
        private static void LogInfo(string message)
        {
            Console.WriteLine(string.Format("[INFO] {0}", message));
        }

        // Hàm Log Error giả định
        private static void LogError(string message)
        {
            Console.Error.WriteLine(string.Format("[ERROR] {0}", message));
        }

        /// <summary>
        /// Mixes a podcast audio file with a background music file using an external FFmpeg process.
        /// This requires the FFmpeg executable to be available in the system PATH.
        /// </summary>
        /// <param name="podcastPath">Path to the main podcast audio file.</param>
        /// <param name="musicPath">Path to the background music file.</param>
        /// <param name="outputPath">Path to save the mixed audio file (MP3 format).</param>
        /// <param name="musicVolumeDb">Volume change for the music track in dB (e.g., -15).</param>
        /// <returns>The output path if successful, otherwise the podcast path.</returns>
        public static string MixAudioWithMusic(string podcastPath, string musicPath, string outputPath, double musicVolumeDb = -15)
        {
            LogInfo("🎵 Mixing background music...");

            if (!File.Exists(podcastPath) || !File.Exists(musicPath))
            {
                LogError("❌ Error: One or both input files not found.");
                return podcastPath;
            }

            try
            {
                // Các đối số FFmpeg cần thiết:
                // 1. Lặp lại nhạc nền: -stream_loop -1 (lặp vô hạn)
                // 2. Cắt nhạc nền: -t [độ dài podcast]
                // 3. Điều chỉnh âm lượng nhạc nền: -filter_complex "[1:a]volume=...dB[bgaudio]"
                // 4. Trộn: -filter_complex "[0:a][bgaudio]amix=inputs=2:duration=first"
                //    (amix: trộn, duration=first: cắt theo độ dài của input đầu tiên (podcast))
                // 5. Chuẩn hóa: -c:a libmp3lame -q:a 2 (encoder và chất lượng)

                // Bước 1: Lấy độ dài của podcast (cần FFprobe, đi kèm FFmpeg)
                // Việc lấy độ dài phức tạp hơn, nên ta sẽ dựa vào tùy chọn amix:duration=first của FFmpeg
                // để nó tự động cắt.

                // Chuyển đổi dB thành dạng số thập phân cho filter volume của FFmpeg
                string volumeFactor = Math.Pow(10.0, musicVolumeDb / 20.0).ToString("0.000");

                // Xây dựng chuỗi lệnh FFmpeg
                string arguments = string.Format(
                    "-i \"{0}\" -stream_loop -1 -i \"{1}\" " + // Input files (podcast, music lặp vô hạn)
                    "-filter_complex \"[1:a]volume={2}[bgaudio];[0:a][bgaudio]amix=inputs=2:duration=first[aout]\" " + // Lọc và trộn
                    "-map \"[aout]\" " + // Ánh xạ luồng âm thanh đầu ra đã trộn
                    "-c:a libmp3lame -q:a 2 -y \"{3}\"", // Encoder, chất lượng, ghi đè, và tệp đầu ra
                    podcastPath,
                    musicPath,
                    volumeFactor,
                    outputPath
                );

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg", // Cần có trong PATH hoặc chỉ định đường dẫn đầy đủ
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true
                    // Có thể thêm RedirectStandardOutput/Error để đọc kết quả log
                };

                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        LogInfo(string.Format("✅ Audio mixed and saved to {0}", outputPath));
                        return outputPath;
                    }
                    else
                    {
                        LogError(string.Format("❌ FFmpeg failed with exit code {0}. Please check your FFmpeg installation and arguments.", process.ExitCode));
                        return podcastPath;
                    }
                }
            }
            catch (Exception e)
            {
                LogError(string.Format("❌ Error mixing audio: {0}", e.Message));
                return podcastPath;
            }
        }

    }



}

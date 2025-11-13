using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Helpers
{
    public class StoryVideoCreationHelper
    {
        // Default values
        private const int DEFAULT_FPS = 24;
        private const int DEFAULT_DURATION = 5; // seconds per image

        // Assume a temporary directory for output and intermediate files
        // In a real application, this should be configurable or managed
        private string _tempDir;
        private string _ffmpegPath; // Path to ffmpeg.exe

        public StoryVideoCreationHelper(string tempDirectory = null, string ffmpegExecutablePath = "ffmpeg")
        {
            _tempDir = tempDirectory ?? Path.Combine(Path.GetTempPath(), "VideoCreatorTemp");
            _ffmpegPath = ffmpegExecutablePath;

            if (!Directory.Exists(_tempDir))
            {
                Directory.CreateDirectory(_tempDir);
            }

            // Verify ffmpeg exists
            //if (!File.Exists(_ffmpegPath) && !IsFfmpegInPath())
            //{
            //    throw new FileNotFoundException($"FFmpeg executable not found at '{_ffmpegPath}' or in system PATH. Please ensure ffmpeg is installed and accessible.");
            //}
        }

        // Helper to check if ffmpeg is in the system's PATH
        private bool IsFfmpegInPath()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = "-version",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit(5000); // Wait up to 5 seconds
                    return process.ExitCode == 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Creates a video from a sequence of images with optional audio.
        /// </summary>
        /// <param name="imagePaths">List of paths to the image files.</param>
        /// <param name="audioPath">Path to the background audio file (optional).</param>
        /// <param name="fps">Frames per second for the output video.</param>
        /// <param name="durationPerImage">How long each image should be displayed in seconds.</param>
        /// <returns>Path to the created video file.</returns>
        /// <exception cref="ArgumentException">If no images are provided.</exception>
        /// <exception cref="FileNotFoundException">If any image path or audio path is invalid.</exception>
        /// <exception cref="Exception">If video creation fails.</exception>
        public string CreateVideo(
            List<string> imagePaths,
            string audioPath = null,
            int fps = DEFAULT_FPS,
            int durationPerImage = DEFAULT_DURATION)
        {
            Console.WriteLine($"Creating video with {imagePaths.Count} images, fps={fps}, duration={durationPerImage}s/image"); // Basic logging

            string tempImageSequenceDir = string.Empty;

            if (imagePaths == null || imagePaths.Count == 0)
            {
                Console.Error.WriteLine("Cannot create video with no images.");
                throw new ArgumentException("Cannot create video with no images.");
            }

            // Verify all image paths exist before processing
            foreach (string imgPath in imagePaths)
            {
                if (!File.Exists(imgPath))
                {
                    Console.Error.WriteLine($"Image file not found: {imgPath}");
                    throw new FileNotFoundException($"Image file not found: {imgPath}");
                }
            }

            // Verify audio path if provided
            bool hasAudio = false;
            if (!string.IsNullOrEmpty(audioPath))
            {
                if (!File.Exists(audioPath))
                {
                    Console.Error.WriteLine($"Audio path provided ({audioPath}) but file not found. Creating video without audio.");
                    audioPath = null; // Proceed without audio
                }
                else
                {
                    hasAudio = true;
                }
            }

            try
            {
                // Create a unique output filename
                string outputFileName = $"story_video_{Guid.NewGuid().ToString()}.mp4";
                string outputPath = Path.Combine(_tempDir, outputFileName);

                // Prepare temporary image sequence directory
                tempImageSequenceDir = Path.Combine(_tempDir, $"images_{Guid.NewGuid().ToString()}");
                Directory.CreateDirectory(tempImageSequenceDir);

                Size? targetSize = null;
                Console.WriteLine("Loading and processing images for video...");

                int numbers = 0;
                // Process images: ensure consistent size and save as a sequence
                for (int i = 0; i < imagePaths.Count; i++)
                {
                    string imgPath = imagePaths[i];
                    try
                    {
                        using (Image originalImage = Image.FromFile(imgPath))
                        {
                            Bitmap processedImage = new Bitmap(originalImage); // Work with a copy

                            if (targetSize == null)
                            {
                                targetSize = processedImage.Size;
                                Console.WriteLine($"Video frame size set to: {targetSize}");
                            }
                            else if (processedImage.Size != targetSize)
                            {
                                Console.WriteLine($"Image {i} ({imgPath}) has size {processedImage.Size}, resizing to {targetSize}");
                                // Resize using high-quality interpolation
                                processedImage = new Bitmap(processedImage, targetSize.Value);
                            }

                            // Save images to a sequential format FFmpeg can understand
                            // %05d means 5 digits, padded with zeros (e.g., 00001.png)

                            for (int z = 0; z < durationPerImage * fps; z++) {
                                string seqImagePath = Path.Combine(tempImageSequenceDir, $"img_{numbers:D5}.png");
                                processedImage.Save(seqImagePath, ImageFormat.Png);
                                numbers++;
                            }

                           
                        }
                    }
                    catch (Exception imgErr)
                    {
                        Console.Error.WriteLine($"Error processing image {imgPath}: {imgErr.Message}");
                        throw new Exception($"Failed to load or process image: {imgPath}", imgErr);
                    }
                }

                if (targetSize == null)
                {
                    Console.Error.WriteLine("No valid images could be processed.");
                    throw new InvalidOperationException("No valid images could be processed.");
                }

                // FFmpeg command to create video from image sequence
                // -r {fps} sets input framerate for images
                // -i "{tempImageSequenceDir}\img_%05d.png" specifies input image sequence
                // -c:v libx264 -vf "fps={fps},format=yuv420p" sets video codec and pixel format for compatibility
                // -t {totalDuration} sets total video duration
                // -y overwrites output file if it exists
                double totalVideoDuration = imagePaths.Count * durationPerImage;

                List<string> ffmpegArgs = new List<string>
                {
                    "-y", // Overwrite output file without asking
                    "-r", fps.ToString(), // Input frame rate
                    "-i", $"\"{tempImageSequenceDir}\\img_%05d.png\"", // Input image sequence
                    "-c:v", "libx264", // Video codec
                    "-pix_fmt", "yuv420p", // Pixel format for broad compatibility
                    // No need for -t here, ffmpeg will use all input frames
                    $"\"{outputPath}\"" // Output path
                };

                // Add audio if available
                if (hasAudio)
                {
                    // FFprobe to get audio duration to correctly loop/trim
                    //double audioDuration = GetMediaDuration(audioPath);
                    double audioDuration = MediaInfoHelper.GetDuration(audioPath);
                    if (audioDuration <= 0)
                    {
                        Console.Error.WriteLine($"Could not determine duration for audio file '{audioPath}'. Proceeding without audio.");
                        hasAudio = false; // Disable audio if duration cannot be found
                    }
                    else
                    {
                        ffmpegArgs.Insert(1, "-i"); // Insert before -i for images
                        ffmpegArgs.Insert(2, $"\"{audioPath}\"");

                        // Map video and audio streams, loop audio if shorter, trim if longer
                        // -shortest ensures the video ends when the shortest stream ends (usually video in this case)
                        // -map 0:v:0 maps the first video stream from the first input (images)
                        // -map 1:a:0 maps the first audio stream from the second input (audio file)
                        ffmpegArgs.Add("-c:a");
                        ffmpegArgs.Add("aac"); // Audio codec
                        ffmpegArgs.Add("-b:a");
                        ffmpegArgs.Add("128k"); // Audio bitrate
                        ffmpegArgs.Add("-vf");
                        ffmpegArgs.Add($"fps={fps},format=yuv420p"); // Ensure filter chain correctly applied
                        ffmpegArgs.Add("-shortest"); // Ensure video ends when shortest stream ends
                    }
                }

                // If only images and no audio path was valid, ensure total duration is set based on images
                if (!hasAudio)
                {
                    ffmpegArgs.Add("-t");
                    ffmpegArgs.Add(totalVideoDuration.ToString());
                }


                string arguments = string.Join(" ", ffmpegArgs);
                Console.WriteLine($"FFmpeg command: {_ffmpegPath} {arguments}");

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = _ffmpegPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();

                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.OutputDataReceived += (sender, e) => { if (e.Data != null) output.AppendLine(e.Data); };
                    process.ErrorDataReceived += (sender, e) => { if (e.Data != null) error.AppendLine(e.Data); };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        Console.Error.WriteLine($"FFmpeg video creation failed with exit code {process.ExitCode}.");
                        Console.Error.WriteLine("FFmpeg Output:");
                        Console.Error.WriteLine(output.ToString());
                        Console.Error.WriteLine("FFmpeg Error:");
                        Console.Error.WriteLine(error.ToString());
                        throw new Exception($"FFmpeg video creation failed. Error: {error.ToString()}");
                    }
                }

                Console.WriteLine($"Successfully created video: {outputPath}");
                return outputPath;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating video: {ex.Message}");
                Console.Error.WriteLine(ex.StackTrace);
                throw new Exception($"Failed to create video: {ex.Message}", ex);
            }
            finally
            {
                // Clean up temporary image sequence directory
                if (Directory.Exists(tempImageSequenceDir))
                {
                    try
                    {
                        //Directory.Delete(tempImageSequenceDir, true);
                        Console.WriteLine($"Cleaned up temporary image directory: {tempImageSequenceDir}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error cleaning up temporary directory {tempImageSequenceDir}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Uses FFprobe (part of FFmpeg) to get the duration of a media file.
        /// </summary>
        /// <param name="mediaPath">Path to the media file.</param>
        /// <returns>Duration in seconds, or -1 if unable to determine.</returns>
        private double GetMediaDuration(string mediaPath)
        {
            try
            {
                // Ensure ffprobe is available
                string ffprobePath = _ffmpegPath.Replace("ffmpeg.exe", "ffprobe.exe");
                if (!File.Exists(ffprobePath) && !IsFfprobeInPath())
                {
                    Console.Error.WriteLine($"FFprobe executable not found at '{ffprobePath}' or in system PATH. Cannot determine media duration.");
                    return -1;
                }

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = ffprobePath,
                    Arguments = $"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 \"{mediaPath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0 && double.TryParse(output, out double duration))
                    {
                        return duration;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error getting media duration for '{mediaPath}': {ex.Message}");
            }
            return -1;
        }

        // Helper to check if ffprobe is in the system's PATH
        private bool IsFfprobeInPath()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "ffprobe",
                    Arguments = "-version",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit(5000); // Wait up to 5 seconds
                    return process.ExitCode == 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

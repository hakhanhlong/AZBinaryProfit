using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Helpers
{
    public class ChunkTextHelper
    {
        private const int CHUNK_SIZE_LIMIT = 4500;

        public static List<string> ChunkAudioScript(string scripts)
        {
            // Split the script into lines
            string[] scriptLines = scripts.Split(new[] { '\n' }, StringSplitOptions.None);

            List<string> scriptChunks = new List<string>();
            string currentChunk = "";

            foreach (string line in scriptLines)
            {
                
                int potentialNewLength = currentChunk.Length + (currentChunk.Length > 0 ? 1 : 0) + line.Length;

                if (potentialNewLength > CHUNK_SIZE_LIMIT)
                {
                    
                    if (!string.IsNullOrEmpty(currentChunk))
                    {
                        scriptChunks.Add(currentChunk);
                    }

                    
                    currentChunk = line;
                }
                else
                {
                    
                    if (!string.IsNullOrEmpty(currentChunk))
                    {
                        
                        currentChunk = $"{currentChunk}\n{line}";
                    }
                    else
                    {
                        
                        currentChunk = line;
                    }
                }
            }

            
            if (!string.IsNullOrEmpty(currentChunk))
            {
                scriptChunks.Add(currentChunk);
            }

            return scriptChunks;
        }
    }
}

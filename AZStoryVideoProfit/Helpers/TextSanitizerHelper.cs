using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Helpers
{
    public class TextSanitizerHelper
    {
        private static readonly Regex EmojiPattern = new Regex(
       "[\u2600-\u27BF\u1F300-\u1F6FF\u1F900-\u1F9FF\u1FA00-\u1FAFF\u1F700-\u1F7FF\u1F800-\u1F8FF\u1F600-\u1F64F]",
       RegexOptions.Compiled | RegexOptions.IgnoreCase);

        
        private static readonly Regex WhitespacePattern = new Regex(@"[\s\t\r\f\v\u00A0]+", RegexOptions.Compiled);

        /// <summary>
        /// Removes emojis and normalizes text for TTS processing.
        /// </summary>
        /// <param name="text">The input string.</param>
        /// <returns>The sanitized string.</returns>
        public static string SanitizeForTts(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string sanitizedText = EmojiPattern.Replace(text, string.Empty);

            sanitizedText = sanitizedText.Normalize(NormalizationForm.FormKC);

            sanitizedText = sanitizedText.Replace("*", string.Empty);

            sanitizedText = WhitespacePattern.Replace(sanitizedText, " ");

            sanitizedText = sanitizedText.Trim();

            sanitizedText = sanitizedText
                .Replace('“', '"')
                .Replace('”', '"')
                .Replace('’', '\'')
                .Replace('—', '-')
                .Replace('–', '-');

            return sanitizedText;
        }
    }
}

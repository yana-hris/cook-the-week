namespace CookTheWeek.Data.Models.Extensions
{
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        /// <summary>
        /// A function to format a string in a way that all meaningful words are capitalized and all punctuation marks are retained, white spaces are trimmed
        /// </summary>
        /// <param name="input">The string to be capitalized</param>
        /// <returns></returns>
        public static string ToAllWordsFirstCapitalLetter(this string input)
        {
            input = Regex.Replace(input, @"\s{2,}", " ");
            string[] words = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string[] excludedWords = { "of", "for", "a", "an", "is", "are", "and", "with", "without", ",", ":", ";", "if", "it" };

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < words.Length; i++)
            {
                // Convert the word to lowercase for comparison
                string word = words[i].ToLower();

                // Capitalize the first letter of each word, except for excluded words
                if (i == 0 || (!excludedWords.Contains(word) && (i > 0 && words[i - 1].EndsWith("."))))
                {
                    // Capitalize the first letter of the word
                    word = char.ToUpper(word[0]) + word.Substring(1);
                }

                // Add the word to the result string
                result.Append(word);

                // Check if the word is followed by punctuation marks
                if (i < words.Length - 1 && !char.IsLetterOrDigit(words[i + 1].First()) && !excludedWords.Contains(words[i + 1].ToLower()))
                {
                    // If yes, add the punctuation mark without any space
                    result.Append(words[i + 1].First());
                    // Skip the punctuation mark
                    i++;
                }
                else
                {
                    // If no punctuation mark or excluded word, add a space
                    result.Append(" ");
                }
            }

            return result.ToString().TrimEnd(); // Remove any trailing space
        }

        /// <summary>
        /// A function to format the user input, so that only the first letter of a string is capitalized, all others are made small
        /// </summary>
        /// <param name="input">The string to be capitalized</param>
        /// <returns></returns>
        //public static string ToOnlyFirstCapitalLetter(this string input)
        //{
        //    input.Trim();
        //    input = Regex.Replace(input, @"\s{2,}", " ");
        //    return input[0].ToString().ToUpper() + input.Substring(1).ToLower();
        //}
    }
}

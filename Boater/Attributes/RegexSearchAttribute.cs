using System;

namespace Boater.Attributes
{
    /// <summary>
    /// Used on class properties and holds a regex that can find the data within a raw input.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class RegexSearchAttribute : Attribute
    {
        /// <summary>
        /// Backing field
        /// </summary>
        private readonly string regex;

        /// <summary>
        /// The maximum number of matches that the pattern <see cref="Regex"/> will match.
        /// </summary>
        /// <remarks>Exactly 2 since the first match is the entire pattern and the second is the capture group.</remarks>
        public const int MAX_MATCHES = 2;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="regex">The regex pattern to find</param>
        public RegexSearchAttribute(string regex)
        {
            this.regex = regex;
        }

        /// <summary>
        /// The regex used to match the data
        /// </summary>
        public string Regex
        {
            get { return regex; }
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace PIEM.Common.Model
{
    /// <summary>
    /// [Not Using]
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Language ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Language Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Language Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// If IsActive is false, the language is obsoleted
        /// </summary>
        public bool IsActive { get; set; }
    }
}

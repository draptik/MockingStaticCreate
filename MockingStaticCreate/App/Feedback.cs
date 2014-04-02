using System;

namespace MockingStaticCreate.App
{
    public class Feedback
    {
        private int? quality;

        /// <summary>
        ///     Value in percentage (between 0 and 100).
        /// </summary>
        public int? Quality 
        { 
            get { return quality; } 
            set
            {
                if (value < 0 || value > 100) {
                    throw new ArgumentOutOfRangeException("value", "value must be between 0 and 100!");
                }
                quality = value; 
            } 
        }
    }
}
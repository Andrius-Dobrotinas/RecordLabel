using System;

namespace RecordLabel
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Returns input Stream as array of bytes and returns null the stream is empty
        /// </summary>
        /// <param name="returnNullIfEmpty">Return null if the stream is empty. Otherwise an empty array of bytes will be returned</param>
        /// <returns></returns>
        public static byte[] ToArray(this System.IO.Stream input, bool returnNullIfEmpty)
        {
            if (returnNullIfEmpty && input.Length == 0)
            {
                return null;
            }
            else
            {
                return input.ToArray();
            }
        }

        /// <summary>
        /// Returns input Stream as array of bytes
        /// </summary>
        public static byte[] ToArray(this System.IO.Stream input)
        {
            if (input.CanSeek && input.Position > 0)
            {
                input.Seek(0, System.IO.SeekOrigin.Begin);
            }
            using (System.IO.MemoryStream memstream = new System.IO.MemoryStream())
            {
                input.CopyTo(memstream);
                return memstream.ToArray();
            }
        }
    }
}

using System;

namespace Generic.Extensions
{
    public static class LongExtensions
    {
        public static string GetReadableTime(this long ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            return string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
        }
    }
}

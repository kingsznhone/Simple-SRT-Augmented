using System;
using System.Diagnostics;

public class VideoStopwatch : Stopwatch
{
    
    public TimeSpan StartOffset { get; set; }

    public VideoStopwatch(TimeSpan _StartOffset)
    {
        StartOffset = _StartOffset;
    }

    public double Seconds
    {
        get
        {
            double Milli = (double)base.ElapsedMilliseconds + (double)StartOffset.TotalMilliseconds;

            return Milli / 1000d;
        }
    }
    public new void Reset()
    {
        StartOffset = new TimeSpan(0);
        base.Reset();
    }
}

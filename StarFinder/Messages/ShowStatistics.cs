using System;

namespace StarFinder.Messages
{
    public class ShowStatistics
    {
        public TimeSpan Duration { get; }

        public ShowStatistics(TimeSpan duration)
        {
            Duration = duration;
        }
    }
}
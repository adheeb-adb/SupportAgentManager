using System;
using SupportAgentManager.Domain.Enums;

namespace SupportAgentManager.Domain.Entities
{
    public class SupportShift
    {
        /* TODO: a bool 'DoesSpanMidNight' should be added to support shifts spanning accross mid night.
         * Ticks for one day () should be added to EndTime, if above is set to true
         */

        public SupportShiftType ShiftType { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}

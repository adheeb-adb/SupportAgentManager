using System;
using System.Collections.Generic;
using System.Linq;
using SupportAgentManager.Domain.Contracts;
using SupportAgentManager.Domain.Dto.ConfigInformation;
using SupportAgentManager.Domain.Entities;

namespace SupportAgentManager.Services
{
    public class SupportShiftService : ISupportShiftService
    {
        private readonly ShiftsConfiguration _shiftsConfiguration;

        public SupportShiftService(ShiftsConfiguration shiftsConfiguration)
        {
            _shiftsConfiguration = shiftsConfiguration;

            InitializeService();
        }

        public List<SupportShift> SupportShifts { get; private set; }

        public SupportShift CurrentShift { get; private set; }

        public void InitializeService() 
        {
            SetSupportShifts(GetSupportShiftsFromConfiguration(_shiftsConfiguration));
            SetCurrentShift();
        }

        public void BeginNewShift()
        {
            SetCurrentShift();
        }

        #region: private methods

        private List<SupportShift> GetSupportShiftsFromConfiguration(ShiftsConfiguration shiftsConfiguration)
        {
            SupportShift businessShift = new SupportShift
            {
                ShiftType = shiftsConfiguration.BusinessShift.ShiftType,
                StartTime = GetTimeSpanFromFormattedString(shiftsConfiguration.BusinessShift.StartTime),
                EndTime = GetTimeSpanFromFormattedString(shiftsConfiguration.BusinessShift.EndTime)
            };

            SupportShift midShift = new SupportShift
            {
                ShiftType = shiftsConfiguration.MidShift.ShiftType,
                StartTime = GetTimeSpanFromFormattedString(shiftsConfiguration.MidShift.StartTime),
                EndTime = GetTimeSpanFromFormattedString(shiftsConfiguration.MidShift.EndTime)
            };

            SupportShift nightShift = new SupportShift
            {
                ShiftType = shiftsConfiguration.NightShift.ShiftType,
                StartTime = GetTimeSpanFromFormattedString(shiftsConfiguration.NightShift.StartTime),
                EndTime = GetTimeSpanFromFormattedString(shiftsConfiguration.NightShift.EndTime)
            };

            return new List<SupportShift> { businessShift, midShift, nightShift };
        }

        private void SetSupportShifts(List<SupportShift> supportShifts)
        {
            this.SupportShifts = supportShifts;
        }

        /* TODO: Currently, shifts spanning accross mid night is not supported
         * In order to handle this, a new property, a bool 'DoesSpanMidNight' should be added to SupportShift
         * Ticks for one day () should be added to EndTime
         */
        private void SetCurrentShift()
        {
            TimeSpan now = DateTime.Now.TimeOfDay;

            SupportShift currentShift = this.SupportShifts.First(s => s.StartTime <= now && now <= s.EndTime);

            this.CurrentShift = currentShift;
        }

        private TimeSpan GetTimeSpanFromFormattedString(string formattedString)
        {
            var timeElements = formattedString.Split(':');
            int hour = int.Parse(timeElements[0]);
            int minute = int.Parse(timeElements[1]);
            int second = int.Parse(timeElements[2]);

            return new TimeSpan(hour, minute, second);
        }

        #endregion
    }
}

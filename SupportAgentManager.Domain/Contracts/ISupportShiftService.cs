using System.Collections.Generic;
using SupportAgentManager.Domain.Entities;

namespace SupportAgentManager.Domain.Contracts
{
    /// <summary>
    /// Interface to define properties and methods of support shifts.
    /// </summary>
    public interface ISupportShiftService
    {
        /// <summary>
        /// Gets list of support shifts..
        /// </summary>
        List<SupportShift> SupportShifts { get; }

        /// <summary>
        /// Gets the current support shift.
        /// </summary>
        SupportShift CurrentShift { get; }

        /// <summary>
        /// Initializes the service bu setting SupportShifts and CurrentShift. To be called during startup or shift configuration change.
        /// </summary>
        /// <param name="supportShifts"> list of support shifts. </param>
        void InitializeService();

        /// <summary>
        /// Sets the current active shift. To be called at  and beginning of each shift.
        /// </summary>
        void BeginNewShift();
    }
}

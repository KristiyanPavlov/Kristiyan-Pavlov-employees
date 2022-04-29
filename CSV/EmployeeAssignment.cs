using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV
{
    public class EmployeeAssignment
    {
        private DateTime mDateTo;

        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo
        {
            get
            {
                return mDateTo;
            }
            set
            {
                if (value < DateFrom)
                    throw new ArgumentOutOfRangeException("Value of DateTo cannot be earlier than DateFrom.");

                mDateTo = value;
            }
        }

        /// <summary>
        /// Get overlap between this and other EmployeeAssignment
        /// </summary>
        /// <param name="otherAssignment"></param>
        /// <returns>TimeSpan</returns>
        public TimeSpan GetOverlap(EmployeeAssignment otherAssignment)
        {
            var startDate = this.DateFrom > otherAssignment.DateFrom ? this.DateFrom : otherAssignment.DateFrom;
            var endDate = this.mDateTo < otherAssignment.DateTo ? this.DateTo : otherAssignment.DateTo;

            if (startDate < endDate)
            {
                return endDate - startDate;
            }

            return TimeSpan.Zero;
        }
    }
}

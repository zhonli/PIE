using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database
{
    /* Frequency: 0, send email once
     * Day: We will use it to send email weekly and monthly later.
     *      When sending email weekly. the value range is 0-6
     *      When monthly, the value range is 0-31
     * Time: We will use it to send email weekly and monthly later.
     */
    public class EmailSchedule
    {
        [Key]
        public int ID { get; set; }

        public int EmailID { get; set; }

        public int Frequency { get; set; }

        public int Day { get; set; }

        public DateTime? Time { get; set; }

        public int State { get; set; }

        [MaxLength(2000)]
        public string Remark { get; set; }

        public virtual Email Email { get; set; }
    }
}

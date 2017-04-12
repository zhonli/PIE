using PIEM.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PIEM.API.Services
{
    public class PlanWarning
    {

        private Plan current;

        private IDictionary<DateTime, IList<Plan>> conflicts = new Dictionary<DateTime, IList<Plan>>();


        public PlanWarning(Plan checking)
        {
            this.current = checking;
        }

        public void AddConflicts(IList<Plan> plans, DateTime date)
        {
            foreach (var plan in plans)
            {
                if (!this.conflicts.ContainsKey(date))
                {
                    this.conflicts.Add(date, plans);
                    break;
                }

                if (this.conflicts[date].Contains(plan))
                    continue;
                this.conflicts[date].Add(plan);

            }
        }

        public async Task Notify()
        {
            try
            {
                await SendEmail();
            }
            catch (Exception ex)
            {
                //TODO:log exception
            }
        }

        private async Task<bool> SendEmail()
        {
            Message.MailServiceClient client = new Message.MailServiceClient();
            Message.EmailInfo email = new Message.EmailInfo();
            email.Title = "Workload Over Warning";
            ResourceService rs = new ResourceService();

            email.To = rs.GetEmailString(current.CreateBy);
            email.Cc = rs.GetEmailString(GetConfilctOwners().ToArray());
            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.Append("Workload over the max of all resource per day");
            htmlBuilder.Append("<br/>");
            htmlBuilder.AppendFormat("<b>Creating Plan</b>: {0} [{1} ~ {2}]", this.current.Title, this.current.StartDate.Value.ToShortDateString(), this.current.EndDate.Value.ToShortDateString());
            htmlBuilder.Append("<br/>");
            htmlBuilder.Append("<b>Conflicts: <b>");

            foreach (var con in this.conflicts)
            {
                htmlBuilder.Append("<div><table>");
                htmlBuilder.AppendFormat("<thead><tr><th>{0}</th></tr></thead>", con.Key.ToShortDateString());
                htmlBuilder.Append("<tbody>");
                foreach (var plan in con.Value)
                {
                    htmlBuilder.AppendFormat("<tr><td>{0}</td></tr>", plan.Title);
                }
                htmlBuilder.Append("</tbody>");

                htmlBuilder.Append("</table></div>");
            }

            email.Html = htmlBuilder.ToString();

            return await client.SendEmailAsync(email);
        }

        public IList<string> GetConfilctOwners()
        {
            IList<string> owners = new List<string>();
            foreach (var con in this.conflicts)
            {
                foreach (var plan in con.Value)
                {
                    if (owners.Contains(plan.CreateBy))
                        continue;
                    owners.Add(plan.CreateBy);
                }
            }

            return owners;

        }

    }
}
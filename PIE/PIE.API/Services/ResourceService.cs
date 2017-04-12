using PIEM.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PIEM.API.Services
{
    public class ResourceService
    {

        public IList<string> GetEmails(IList<string> alias)
        {
            IList<string> emails = new List<string>();

            using (PIEMContext context = new PIEMContext())
            {
                foreach (var al in alias)
                {
                    var resource = context.Resources.FirstOrDefault(r => r.Alias.ToUpper() == al.ToUpper());
                    if (resource == null)
                        continue;
                    emails.Add(resource.Email);
                }
            }

            return emails;
        }

        public string GetEmailString(params string[] alias)
        {
            StringBuilder sb = new StringBuilder();

            using (PIEMContext context = new PIEMContext())
            {
                foreach (var al in alias)
                {
                    var resource = context.Resources.FirstOrDefault(r => r.Alias.ToUpper() == al.ToUpper());
                    if (resource == null)
                        continue;
                    sb.AppendFormat("{0};", resource.Email);
                }
            }

            return sb.ToString().TrimEnd(';');
        }
    }
}
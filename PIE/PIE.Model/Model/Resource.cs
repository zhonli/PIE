using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIEM.Common.Model
{
    /// <summary>
    /// Defined User realted info
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// User ID
        /// </summary>
        public int ResourceID { get; set; }
        /// <summary>
        /// User Alias
        /// As, v-biyli
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// User Domain
        /// As, EUROPE
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// User Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// [Not Using]
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// [Not Using]
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// [Not Using]
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// User Display Name
        /// As, Jing Gao (Pactera Technologies Inc)
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// [Not Using]
        /// </summary>
        public Role Role { get; set; }
        /// <summary>
        /// [New Feature]
        /// </summary>
        public virtual IList<Team> Teams { get; set; }
        /// <summary>
        /// The Assignments of the resouce 
        /// </summary>
        public virtual IList<Assignment> Assignments { get; set; }
    }
}

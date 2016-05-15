using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Core;
using BrightLine.Common.Models;

namespace BrightLine.Tests.Samples
{
    /// <summary>
    /// Sample class for unit-tests examples ( BrightLine Account )
    /// </summary>
    public class BLAccount : EntityBase, IEntity
    {
        public string Name;
        public bool IsActive;
        public virtual ICollection<Campaign> Campaigns { get; set; }


        public BLAccount()
        {
        }


        public BLAccount(string name, bool isActive)
        {
            Name = name;
            IsActive = isActive;
        }
    }
}

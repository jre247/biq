using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BrightLine.Core;
using BrightLine.Common.Services;


namespace BrightLine.Tests.Samples
{
    public interface IBLAccountService : ICrudService<BLAccount>
    {
        List<string> GetAllRoles();

        string GetRoleFor(string user);

        string GetFullName(string user);
    }
}

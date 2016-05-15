using System.Collections.Generic;
using System.Linq;
using BrightLine.Core;
using BrightLine.Common.Models;
using BrightLine.Common.Framework;


namespace BrightLine.Tests.Samples
{
    public class BLAccountService : CrudService<BLAccount>, IBLAccountService
    {
        public BLAccountService()
            : base(null)
        {
        }


        public List<string> GetAllRoles()
        {
            return new List<string>() { "admin", "moderator" };
        }


        public string GetRoleFor(string user)
        {
            return "admin";
        }


        public string GetFullName(string user)
        {
            return "user : " + user.ToLower();
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="repo"></param>
        public void SetupRepo(IRepository<BLAccount> repository)
        {
            repo = repository;
        }


        /// <summary>
        /// The campaign repo ( option to set it explicity for examples for unit tests )
        /// </summary>
        public IRepository<Campaign> CampaignRepo { get; set; }



        /// <summary>
        /// This example uses the Repositories assigned to this service
        /// INSTEAD OF using the IOC container. This is just to illustrate accessing
        /// data from different repos.
        /// </summary>
        /// <returns></returns>
        public int Avg_Without_IOC()
        {
            return Avg(this.CampaignRepo);
        }


        /// <summary>
        /// This example uses the Repositories assigned to this service
        /// INSTEAD OF using the IOC container. This is just to illustrate accessing
        /// data from different repos.
        /// </summary>
        /// <returns></returns>
        public int Avg_With_IOC()
        {
            repo = IoC.Resolve<IRepository<BLAccount>>();
            var campaignRepo = IoC.Resolve<IRepository<Campaign>>();
            return Avg(campaignRepo);
        }
        
        
        /// <summary>
        /// This example uses the Repositories assigned to this service
        /// INSTEAD OF using the IOC container. This is just to illustrate accessing
        /// data from different repos.
        /// </summary>
        /// <returns></returns>
        private int Avg(IRepository<Campaign> campaignRepo)
        {
            // 1: Use the repo for campaign group to get all groups.
            var all = this.GetAll();
            var totalCampaigns = 0;
            var totalGroups = all.Count();

            // Go through all groups to get their campaign
            foreach (var group in all)
            {
                // 2. Now access the campaign repo to get campaigns in this group.
                var campaignsInGroup = campaignRepo.Where(c => c.Spend == group.Id);
                totalCampaigns += campaignsInGroup.Count();
            }
            if (totalGroups == 0)
                return 0;

            return totalCampaigns / totalGroups;
        }
    }
}

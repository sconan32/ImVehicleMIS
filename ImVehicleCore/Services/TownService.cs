using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using ImVehicleCore.Data;
using ImVehicleCore.Interfaces;
using ImVehicleCore.Specifications;

namespace ImVehicleCore.Data
{
    public class TownService : ITownService
    {
        private IAsyncRepository<TownItem> _townRepository;
        private IGroupRepository _groupService;

        public TownService(IAsyncRepository<TownItem> townRepository, IGroupRepository groupService)
        {
            this._townRepository = townRepository;
            this._groupService = groupService;
        }

        public async Task<TownItem> GetTownByUser(string userName)
        {
            Guard.Against.NullOrEmpty(userName, nameof(userName));
            var list = await _townRepository.ListAsync(new TownWithUserSpecification(userName));
            if (list.Any())
            {
                var item = list[0];

                item.Groups = await _groupService.GetGroupsOfTown(item.Id);
                return item;
            }
            return null;

        }
    }
}

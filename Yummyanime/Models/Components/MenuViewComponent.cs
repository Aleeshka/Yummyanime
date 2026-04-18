using Microsoft.AspNetCore.Mvc;
using Yummyanime.Domain;
using Yummyanime.Domain.Entities;
using Yummyanime.Infrastructure;

namespace Yummyanime.Models.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly DataManager _dataManager;

        public MenuViewComponent(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Service> list = await _dataManager.Services.GetServicesAsync();

            IEnumerable<ServiceDTO> listDTO = HelperDTO.TransformServices(list);

            return await Task.FromResult((IViewComponentResult)View("Default", listDTO));
        }
    }
}

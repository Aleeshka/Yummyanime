using Microsoft.AspNetCore.Mvc;
using Yummyanime.Domain;
using Yummyanime.Domain.Entities;
using Yummyanime.Infrastructure;
using Yummyanime.Models;

namespace Yummyanime.Controllers
{
    public class ServicesController : Controller
    {
        private readonly DataManager _dataManager;

        public ServicesController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Service> list = await _dataManager.Services.GetServicesAsync();

            // Доменную сущность на клиенте использовать не рекомендуется, оборачиваем ее в DTO
            IEnumerable<ServiceDTO> listDTO = HelperDTO.TransformServices(list);

            return View(listDTO);
        }

        public async Task<IActionResult> Show(int id)
        {
            Service? entity = await _dataManager.Services.GetServiceByIdAsync(id);

            // Услуги с данным ID не найдено, отвечаем кодом 404
            if (entity is null)
                return NotFound();

            // Доменную сущность на клиенте использовать не рекомендуется, оборачиваем ее в DTO
            ServiceDTO entityDTO = HelperDTO.TransformService(entity);

            return View(entityDTO);
        }
    }
}

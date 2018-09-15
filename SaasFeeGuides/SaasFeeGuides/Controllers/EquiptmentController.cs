using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SaasFeeGuides.Data;
using SaasFeeGuides.Helpers;
using SaasFeeGuides.Models.Entities;
using SaasFeeGuides.ViewModels;


namespace SaasFeeGuides.Controllers
{
    [Authorize("Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class EquiptmentController : ControllerBase
    {

        private readonly IEquiptmentRepository _equiptmentRepository;
        private readonly IContentRepository _contentRepository;

        public EquiptmentController(
            IEquiptmentRepository equiptmentRepository,
             IContentRepository contentRepository)
        {
            _equiptmentRepository = equiptmentRepository;
            _contentRepository = contentRepository;
        }

        [Authorize("Admin")]
        [HttpPut]
        public async Task<IActionResult> AddOrUpdateEquiptment(ViewModels.Equiptment equiptment)
        {
            var equiptmentModel = equiptment.Map(_contentRepository);

            var id = await _equiptmentRepository.UpsertEquiptment(equiptmentModel);

            return new OkObjectResult(id);
        }

    }
}
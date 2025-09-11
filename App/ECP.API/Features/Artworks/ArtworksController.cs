using ECP.Shared;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECP.API.Features.Artworks
{
    [Route("api/artworks")]
    [ApiController]
    public class ArtworksController(IArtworksService artworksService) : ControllerBase
    {
        private readonly IArtworksService _artworksService = artworksService;

        [HttpGet("previews")]
        public async Task<IActionResult> GetArtworkPreviewAsync([FromQuery] int count, int results_per_page, int page_num)
        {
            Shared.Result<PaginatedResponse<ArtworkPreview>> response = await _artworksService.GetArtworkPreviewsAsync(count, results_per_page, page_num);

            return response.IsSuccess switch
            {
                true => (response.Value.Data == null || !response.Value.Data.Any()) ? NoContent() : Ok(response.Value),
                false => StatusCode(500, new { error = response.Message })
            };
        }

        [HttpGet("previews/search")]
        public async Task<IActionResult> GetArtworkPreviewByQueryAsync([FromQuery] string q, string? sort = null, [FromQuery] List<string>? filters = null, int limit = 25, int page = 1)
        {
            Shared.Result<PaginatedResponse<ArtworkPreview>> response = await _artworksService.SearchAllArtworkPreviewsAsync(q, sort, filters, limit, page);

            return response.IsSuccess switch
            {
                true => (response.Value.Data == null || !response.Value.Data.Any()) ? NoContent() : Ok(response.Value),
                false => response.StatusCode == System.Net.HttpStatusCode.BadRequest ? BadRequest(response.Message) : StatusCode(500, new { error = response.Message })
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetArtworkByIdAsync([FromQuery] int id, [FromQuery] ArtworkSource source)
        {
            Shared.Result<Artwork> response = await _artworksService.GetArtworkByIdAsync(id, source);

            return response.IsSuccess switch
            {
                true => (response.Value == null) ? NotFound() : Ok(response.Value),
                false => response.StatusCode == System.Net.HttpStatusCode.BadRequest ? BadRequest(response.Message) : StatusCode(500, new { error = response.Message })
            };
        }



    }
}

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

        // GET: api/<ArtworksController>
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

        // GET: api/<ArtworksController>
        [HttpGet("previews/search")]
        public async Task<IActionResult> GetArtworkPreviewByQueryAsync([FromQuery] string q, int results_per_page, int page_num)
        {
            Shared.Result<PaginatedResponse<ArtworkPreview>> response = await _artworksService.SearchAllArtworkPreviewsAsync(q, results_per_page, page_num);

            return response.IsSuccess switch
            {
                true => (response.Value.Data == null || !response.Value.Data.Any()) ? NoContent() : Ok(response.Value),
                false => StatusCode(500, new { error = response.Message })
            };
        }



    }
}

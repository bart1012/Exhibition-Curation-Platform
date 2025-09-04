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
        public async Task<IActionResult> GetArtworkPreviewAsync([FromQuery] int count)
        {
            Shared.Result<List<ArtworkPreview>> response = await _artworksService.GetArtworkPreviewAsync(count);

            return response.IsSuccess switch
            {
                true => (response.Value == null || !response.Value.Any()) ? NoContent() : Ok(response.Value),
                false => StatusCode(500, new { error = response.ErrorMessage })
            };
        }

        // GET: api/<ArtworksController>
        [HttpGet("previews/search")]
        public async Task<IActionResult> GetArtworkPreviewByQueryAsync([FromQuery] string q, [FromQuery] int count, [FromQuery] int offset)
        {
            Shared.Result<List<ArtworkPreview>> response = await _artworksService.GetArtworkPreviewByQueryAsync(q, count, offset);

            return response.IsSuccess switch
            {
                true => (response.Value == null || !response.Value.Any()) ? NoContent() : Ok(response.Value),
                false => StatusCode(500, new { error = response.ErrorMessage })
            };
        }



    }
}

using Microsoft.AspNetCore.Mvc;
using DiffProject.Api.Models;
using DiffProject.Api.Services;

namespace DiffProject.Api.Controllers;

[ApiController]
[Route("v1/diff/{id}")]
public class DiffController(DiffService service) : ControllerBase
{
    /* PUT / id / side 
     * + data in body
     * sets data and returns:
     * if data is not a B64 value: BadRequest
     * if data is null: BadRequest
     * if data with id and side exists, replaces data on id position, returns: OK
     * else: returns OK
     * -----------------------
     * PUT example v1/diff/1/left
     * body {data:"AAAAAA=="}
     */

    [HttpPut("{side}")]
    public IActionResult PutData(string id, string side, [FromBody] DataInput input)
    {
        if (!Enum.TryParse<DiffSide>(side, true, out var sideEnum)) return NotFound();
        if (string.IsNullOrEmpty(input?.Data)) return BadRequest();

        try
        {
            service.SaveInput(id, input.Data, sideEnum);
            return CreatedAtAction(nameof(GetDiff), new { id }, null);
        }
        catch (InvalidBase64Exception) { return BadRequest("Not a B64 data"); }
    }

    /* calling GET + id returns:
     * if left or right were not set: 404 NotFound
     * if left and right are same: 200 OK, "Equals"
     * if different sizes: 200 OK,  "SizeDoNotMatch"
     * if same sizes and different data: 200 OK, "ContentDoNotMatch" + offsets & lengths
     * ---------------------------------
     * GET example v1/diff/1 
     */
    [HttpGet]
    public IActionResult GetDiff(string id)
    {
        var result = service.GetDiff(id);
        // If the service returns null, we return the 404
        if (result == null) return NotFound();
        return Ok(result);
    }
}

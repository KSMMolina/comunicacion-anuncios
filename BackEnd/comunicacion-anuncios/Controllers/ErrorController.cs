using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace comunicacion_anuncios.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult Handle()
        => Problem(title: "UnhandledError", statusCode: StatusCodes.Status500InternalServerError);
}
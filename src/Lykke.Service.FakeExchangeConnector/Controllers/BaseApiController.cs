using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.FakeExchangeConnector.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public abstract class BaseApiController : Controller
    {
        protected BaseApiController()
        {
        }
    }
}

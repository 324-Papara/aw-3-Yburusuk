using MediatR;
using Microsoft.AspNetCore.Mvc;
using Para.Base.Response;
using Para.Business.Cqrs;
using Para.Schema;

namespace Para.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerReportsController : ControllerBase
{
    private readonly IMediator mediator;
    
    public CustomerReportsController(IMediator mediator)
    {
        this.mediator = mediator;
    }


    [HttpGet("CustomerReports")]
    public async Task<ApiResponse<List<CustomerReportResponse>>> Get()
    {
        var operation = new GetCustomerReportsQuery();
        var result = await mediator.Send(operation);
        return result;
    }
}
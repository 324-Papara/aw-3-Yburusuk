﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Para.Base.Response;
using Para.Business.Cqrs;
using Para.Schema;

namespace Para.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerDetailsController : ControllerBase
{
    private readonly IMediator mediator;

    public CustomerDetailsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<CustomerDetailResponse>>> Get()
    {
        var operation = new GetAllCustomerDetailQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("{customerDetailId}")]
    public async Task<ApiResponse<CustomerDetailResponse>> Get([FromRoute] long customerDetailId)
    {
        var operation = new GetCustomerDetailByIdQuery(customerDetailId);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<CustomerDetailResponse>> Create([FromBody] CustomerDetailRequest value)
    {
        var operation = new CreateCustomerDetailCommand(value);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{customerDetailId}")]
    public async Task<ApiResponse> Update(long customerDetailId, [FromBody] CustomerDetailRequest value)
    {
        var operation = new UpdateCustomerDetailCommand(customerDetailId, value);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{customerDetailId}")]
    public async Task<ApiResponse> Delete(long customerDetailId)
    {
        var operation = new DeleteCustomerDetailCommand(customerDetailId);
        var result = await mediator.Send(operation);
        return result;
    }
}
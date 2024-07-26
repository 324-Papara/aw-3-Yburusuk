using MediatR;
using Para.Base.Response;
using Para.Schema;

namespace Para.Business.Cqrs;

public record GetCustomerReportsQuery() : IRequest<ApiResponse<List<CustomerReportResponse>>>;
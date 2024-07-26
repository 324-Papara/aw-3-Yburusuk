using System.Data;
using Dapper;
using MediatR;
using Para.Base.Response;
using Para.Business.Cqrs;
using Para.Data.Domain;
using Para.Schema;

namespace Para.Business.Query;

public class CustomerReportQueryHandler : IRequestHandler<GetCustomerReportsQuery, ApiResponse<List<CustomerReportResponse>>>
{
    private readonly IDbConnection dbConnection;

    public CustomerReportQueryHandler(IDbConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public async Task<ApiResponse<List<CustomerReportResponse>>> Handle(GetCustomerReportsQuery request, CancellationToken cancellationToken)
    {
        var sql = @"
                    SELECT c.Id as CustomerId, c.*, cd.*, ca.*, cp.*
                    FROM Customer c
                    LEFT JOIN CustomerDetail cd ON c.Id = cd.CustomerId
                    LEFT JOIN CustomerAddress ca ON c.Id = ca.CustomerId
                    LEFT JOIN CustomerPhone cp ON c.Id = cp.CustomerId;
                    ";

        var customerDictionary = new Dictionary<long, CustomerReportResponse>();

        var customers = await dbConnection.QueryAsync<Customer, CustomerDetailResponse,
            CustomerAddressResponse, CustomerPhoneResponse, CustomerReportResponse>(
            sql,
            (Customer, Detail, Address, Phone) =>
            {
                if (!customerDictionary.TryGetValue(Customer.CustomerNumber, out var customerReport))
                {
                    customerReport = new CustomerReportResponse()
                    {
                        FirstName = Customer.FirstName,
                        LastName = Customer.LastName,
                        IdentityNumber = Customer.IdentityNumber,
                        Email = Customer.Email,
                        CustomerNumber = Customer.CustomerNumber,
                        DateOfBirth = Customer.DateOfBirth,
                        CustomerDetail = Detail,
                        CustomerAddresses = new List<CustomerAddressResponse>(),
                        CustomerPhones = new List<CustomerPhoneResponse>()
                    };

                    customerDictionary.Add(Customer.CustomerNumber, customerReport);
                }

                if (Address != null)
                {
                    customerReport.CustomerAddresses.Add(Address);
                }

                if (Phone != null)
                {
                    customerReport.CustomerPhones.Add(Phone);
                }

                return customerReport;
            },
            splitOn: "CustomerId,Id,Id,Id"
        );

        var customersList = customerDictionary.Values.ToList();
        return new ApiResponse<List<CustomerReportResponse>>(customersList);
    }
}
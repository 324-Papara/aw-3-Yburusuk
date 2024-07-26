using Para.Base.Schema;

namespace Para.Schema;

public class CustomerReportResponse : BaseResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdentityNumber { get; set; }
    public string Email { get; set; }
    public int CustomerNumber { get; set; }
    public DateTime DateOfBirth { get; set; }

    public virtual CustomerDetailResponse CustomerDetail { get; set; }
    public virtual List<CustomerPhoneResponse> CustomerPhones { get; set; }
    public virtual List<CustomerAddressResponse> CustomerAddresses { get; set; }
}
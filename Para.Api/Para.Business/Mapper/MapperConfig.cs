using AutoMapper;
using Para.Data.Domain;
using Para.Schema;

namespace Para.Business;

public class MapperConfig : Profile
{

    public MapperConfig()
    {
        CreateMap<Customer, CustomerResponse>();
        CreateMap<CustomerRequest, Customer>();
        
        CreateMap<CustomerAddress, CustomerAddressResponse>();
        CreateMap<CustomerAddressRequest, CustomerAddress>();
        
        CreateMap<CustomerPhone, CustomerPhoneResponse>();
        CreateMap<CustomerPhoneRequest, CustomerPhone>();
        
        CreateMap<CustomerDetail, CustomerDetailResponse>();
        CreateMap<CustomerDetailRequest, CustomerDetail>();
    }
}
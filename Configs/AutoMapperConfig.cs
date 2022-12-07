using AutoMapper;
using CheapUdemy.Data;
using CheapUdemy.Models.User;
//using CheapUdemy.Models.Payment;
using CheapUdemy.Models.Udem;
//using CheapUdemy.Models;


namespace CheapUdemy.Configs
{
    public class AutoMapperConfig : Profile
    {

        public AutoMapperConfig()
        {

            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<UserLogin, CreateUserLoginDto>().ReverseMap();
            CreateMap<UdemAccount, CreateUdemAccountDto>().ReverseMap();
            //CreateMap<Payment, CreatePaymentDto>().ReverseMap();
            CreateMap<UserServicePurchase, CreateUserServicePurchaseDto>().ReverseMap();


        }

    }
}

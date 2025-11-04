using AutoMapper;
using SmartCRM.Application.Dtos.Company_Dtos;
using SmartCRM.Application.Dtos.Customer_Dtos;
using SmartCRM.Application.Dtos.Deal_Dtos;
using SmartCRM.Application.Dtos.DealProduct_Dtos;
using SmartCRM.Application.Dtos.Interaction_Dtos;
using SmartCRM.Application.Dtos.Lead_Dtos;
using SmartCRM.Application.Dtos.Note_Dtos;
using SmartCRM.Application.Dtos.Product_Dtos;
using SmartCRM.Application.Dtos.Ticket_Dtos;
using SmartCRM.Application.Dtos.User_Dtos;
using SmartCRM.Domain.Entities;   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<UpdateCompanyDto, Company>();

            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<UpdateUserDto, User>();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

            CreateMap<Deal, DealDto>().ReverseMap();
            CreateMap<CreateDealDto, Deal>();
            CreateMap<UpdateDealDto, Deal>();

            CreateMap<DealProduct, DealProductDto>().ReverseMap();
            CreateMap<CreateDealProductDto, DealProduct>();

            CreateMap<Note, NoteDto>().ReverseMap();
            CreateMap<CreateNoteDto, Note>();

            CreateMap<Interaction, InteractionDto>().ReverseMap();
            CreateMap<CreateInteractionDto, Interaction>();

            CreateMap<Lead, LeadDto>().ReverseMap();
            CreateMap<CreateLeadDto, Lead>();

            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<CreateTicketDto, Ticket>();
        }
    }
}

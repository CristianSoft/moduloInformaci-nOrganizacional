using AutoMapper;
using backendfepon.DTOs.AdmnistrativeMemberDTOs;
using backendfepon.Models;
using System.Globalization;

namespace backendfepon.ModelConfigurations.Profiles
{
    public class AdministrativeMemberProfile : Profile
    {
        public AdministrativeMemberProfile()
        {
            // Mapping from AdministrativeMember to AdministrativeMemberDTO
             CreateMap<AdministrativeMember, AdministrativeMemberDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Administrative_Member_Id))
                 .ForMember(dest => dest.firstName, opt => opt.MapFrom(src => src.Name))
                 .ForMember(dest => dest.lastName, opt => opt.MapFrom(src => src.Last_Name))
                 .ForMember(dest => dest.birthDate, opt => opt.MapFrom(src => src.Birth_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                 .ForMember(dest => dest.cellphone, opt => opt.MapFrom(src => src.Phone))
                 .ForMember(dest => dest.faculty, opt => opt.MapFrom(src => src.Faculty.Faculty_Name))
                 .ForMember(dest => dest.career, opt => opt.MapFrom(src => src.Career.Career_Name))
                 .ForMember(dest => dest.semester, opt => opt.MapFrom(src => src.Semester.Semester_Name))
                 .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                 .ForMember(dest => dest.position, opt => opt.MapFrom(src => src.Role.Role_Name));

            // Mapping from CreateUpdateAdministrativeMemberDTO to AdministrativeMember
            CreateMap<CreateUpdateAdministrativeMemberDTO, AdministrativeMember>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.Last_Name, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Birth_Date, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Cellphone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Faculty, opt => opt.Ignore()) // Ignora Faculty si no está mapeado
                .ForMember(dest => dest.Career, opt => opt.Ignore()) // Ignora Career si no está mapeado
                .ForMember(dest => dest.Semester, opt => opt.Ignore())
                .ForMember(dest => dest.State, opt => opt.Ignore()) // Ignora Semester si no está mapeado
                .ForMember(dest => dest.Role_Id, opt => opt.Ignore()); // Ignora Role_Id
          
        }
    }
}

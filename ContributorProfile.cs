using AutoMapper;
using backendfepon.DTOs.ContributorDTO;
using backendfepon.Models;
using System.Globalization;

namespace backendfepon.ModelConfigurations.Profiles
{
    public class ContributorProfile : Profile
    {
        public ContributorProfile()
        {
            // Mapping from Contributor to ContributorDTO
            CreateMap<Contributor, ContributorDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Contributor_Id))
                .ForMember(dest => dest.state_id, opt => opt.MapFrom(src => src.State_Id))
                .ForMember(dest => dest.plan, opt => opt.MapFrom(src => src.ContributionPlan.Name))
                .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.Contributor_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.ContributionPlan.Economic_Value))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.faculty, opt => opt.MapFrom(src => src.Faculty.Faculty_Name))
                .ForMember(dest => dest.career, opt => opt.MapFrom(src => src.Career.Career_Name))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.academicPeriod, opt => opt.MapFrom(src => src.ContributionPlan.AcademicPeriod.Academic_Period_Name));



            CreateMap<CreateUpdateContributorDTO, Contributor>()
          .ForMember(dest => dest.Contributor_Date, opt => opt.MapFrom(src => src.Date))
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
          .ForMember(dest => dest.Faculty, opt => opt.Ignore())
          .ForMember(dest => dest.Career, opt => opt.Ignore())
          .ForMember(dest => dest.ContributionPlan, opt => opt.Ignore())
          .ForMember(dest => dest.Faculty_Id, opt => opt.Ignore())
          .ForMember(dest => dest.Career_Id, opt => opt.Ignore())
          .ForMember(dest => dest.Plan_Id, opt => opt.Ignore())
          .ForMember(dest => dest.State_Id, opt => opt.Ignore());
        }
    }
}

using AutoMapper;
using backendfepon.DTOs.ContributionPlanDTOs;
using backendfepon.Models;

namespace backendfepon.ModelConfigurations.Profiles
{
    public class ContributionPlanProfile : Profile
    {
        public ContributionPlanProfile()
        {
            // Mapping from ContributionPlan to ContributionPlanDTO
            CreateMap<ContributionPlan, ContributionPlanDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Plan_Id))
                .ForMember(dest => dest.state_id, opt => opt.MapFrom(src => src.State_Id))
                .ForMember(dest => dest.academicPeriod, opt => opt.MapFrom(src => src.AcademicPeriod.Academic_Period_Name))
                .ForMember(dest => dest.planName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.benefits, opt => opt.MapFrom(src => src.Benefits))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.Economic_Value));

            // Mapping from CreateUpdateContributionPlanDTO to ContributionPlan
            CreateMap<CreateUpdateContributionPlanDTO, ContributionPlan>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.planName))
                .ForMember(dest => dest.Benefits, opt => opt.MapFrom(src => src.benefits))
                .ForMember(dest => dest.Economic_Value, opt => opt.MapFrom(src => src.price))
                .ForMember(dest => dest.State, opt => opt.Ignore())
                .ForMember(dest => dest.AcademicPeriod, opt => opt.Ignore())
                .ForMember(dest => dest.Contributors, opt => opt.Ignore());
        }
    }
}

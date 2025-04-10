using AutoMapper;
using EliteAI.Application.DTOs.Profile;
using EliteAI.Application.DTOs.Sports;
using EliteAI.Application.DTOs.User;
using EliteAI.Domain.Entities;

namespace EliteAI.Application.Mapping;

public class UserMappingProfile : AutoMapper.Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName ?? string.Empty))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName ?? string.Empty))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName ?? string.Empty))
            .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile != null ? MapProfile(src.Profile) : null))
            .ForMember(dest => dest.SportsDto, opt => opt.MapFrom(src =>
                src.Profile != null &&
                src.Profile.Sports != null &&
                src.Profile.Sports.Any()
                    ? MapSports(src.Profile.Sports.First())
                    : null));
    }

    private static ProfileDto MapProfile(Domain.Entities.Profile profile)
    {
        return new ProfileDto
        {
            Id = profile.Id,
            Height = profile.Height,
            Weight = profile.Weight,
            AgeGroup = profile.AgeGroup,
            Gender = profile.Gender,
            AvailableEquipment = profile.AvailableEquipment,
            GymAccess = profile.GymAccess,
            GymExperience = profile.GymExperience,
            Injured = profile.Injured,
            Injuries = profile.Injuries,
            TrainingFrequency = profile.TrainingFrequency
        };
    }

    private static SportsDto MapSports(Domain.Entities.Sports sports)
    {
        return new SportsDto
        {
            Id = sports.Id,
            Sport = sports.Sport,
            SeasonStart = sports.SeasonStart,
            SeasonEnd = sports.SeasonEnd,
            Position = sports.Position,
            SportLevel = sports.SportLevel,
            Goals = sports.Goals
        };
    }
}
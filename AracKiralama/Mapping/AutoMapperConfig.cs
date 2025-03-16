using AracKiralama.Models;
using AracKiralama.Models.ViewModels;
using AutoMapper;

namespace AracKiralama.Mapping
{
	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig()
		{
			CreateMap<AppUser, UserListVM>()
				.ForMember(dest => dest.Roles, opt => opt.Ignore());
			CreateMap<AppUser, GetUserVM>()
				.ForMember(dest => dest.Roles, opt => opt.Ignore());
		}
	}
}

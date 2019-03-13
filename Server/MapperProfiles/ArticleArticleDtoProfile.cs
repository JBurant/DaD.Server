using AutoMapper;
using Server.DTO;
using Server.Models;

namespace Server.MapperProfiles
{
    public class ArticleArticleDtoProfile : Profile
    {
        public ArticleArticleDtoProfile()
        {
            CreateMap<Article, ArticleDTO>()
                .ForMember(dest => dest.ArticleHeader, opts => opts.MapFrom(src => src));
            
            CreateMap<Article, ArticleDTO>().ReverseMap();
        }
    }
}
using AutoMapper;
using Server.DTO;
using Server.Models;

namespace Server.MapperProfiles
{
    public class ArticleArticleHeaderProfile : Profile
    {
        public ArticleArticleHeaderProfile()
        {
            CreateMap<Article, ArticleHeader>();
            CreateMap<Article, ArticleHeader>().ReverseMap();
        }
    }
}
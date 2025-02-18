using MiniAppJuanTemplate.Helpers;
using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.Areas.Manage.Services.Interfaces
{
    public interface ITagService
    {

        PaginatedList<Tag> GetPaginatedTags(int page, int take);
        Tag GetTagById(int id);
        bool CreateTag(Tag tag, out string errorMessage);
        bool UpdateTag(Tag tag, out string errorMessage);
        bool DeleteTag(int id);


    }
}

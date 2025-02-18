using Microsoft.EntityFrameworkCore;
using MiniAppJuanTemplate.Areas.Manage.Services.Interfaces;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Helpers;
using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.Areas.Manage.Services.Implements
{
    public class TagService : ITagService
    {
        private readonly JuanAppDbContext _context;

        public TagService(JuanAppDbContext context)
        {
            _context = context;
        }

        public PaginatedList<Tag> GetPaginatedTags(int page, int take)
        {
            var query = _context.Tags.Include(t => t.ProductsTags);
            return PaginatedList<Tag>.Create(query, take, page);
        }

        public Tag GetTagById(int id)
        {
            return _context.Tags.Include(t => t.ProductsTags).FirstOrDefault(t => t.Id == id);
        }

        public bool CreateTag(Tag tag, out string errorMessage)
        {
            if (_context.Tags.Any(c => c.Name.Trim().ToLower() == tag.Name.Trim().ToLower()))
            {
                errorMessage = "This tag already exists.";
                return false;
            }

            _context.Tags.Add(tag);
            _context.SaveChanges();
            errorMessage = null;
            return true;
        }

        public bool UpdateTag(Tag tag, out string errorMessage)
        {
            var existTag = _context.Tags.Include(t => t.ProductsTags).FirstOrDefault(t => t.Id == tag.Id);
            if (existTag == null)
            {
                errorMessage = "Tag not found.";
                return false;
            }

            if (_context.Tags.Any(t => t.Name.Trim().ToLower() == tag.Name.Trim().ToLower() && t.Id != tag.Id))
            {
                errorMessage = "This tag already exists.";
                return false;
            }

            existTag.Name = tag.Name;
            _context.SaveChanges();
            errorMessage = null;
            return true;
        }

        public bool DeleteTag(int id)
        {
            var tag = _context.Tags.Include(t => t.ProductsTags).FirstOrDefault(t => t.Id == id);
            if (tag == null)
            {
                return false;
            }

            _context.Tags.Remove(tag);
            _context.SaveChanges();
            return true;
        }


    }
}

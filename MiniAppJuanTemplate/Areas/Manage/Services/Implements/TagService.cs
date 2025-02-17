using Microsoft.AspNetCore.Mvc.ModelBinding;
using MiniAppJuanTemplate.Areas.Manage.Services.Interfaces;
using MiniAppJuanTemplate.Data;
using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.Areas.Manage.Services.Implements
{
    //public class TagService : ITagService
    //{
    //    private readonly JuanAppDbContext _juanAppDbContext;
    //    public void Create(Tag tag,ModelStateDictionary ModelState)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return View();
    //        }
    //        if (_juanAppDbContext.Tags.Any(c => c.Name.Trim().ToLower() == tag.Name.Trim().ToLower()))
    //        {
    //            ModelState.AddModelError("Name", "This tag already exist");
    //            return View();
    //        }
    //        _juanAppDbContext.Tags.Add(tag);
    //        _juanAppDbContext.SaveChanges();
    //    }

    //    public void Delete(int? id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Detail(int? id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public List<Product> GetAllProducts()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Product GetProductById()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Update(int? id)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}

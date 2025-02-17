using Microsoft.AspNetCore.Mvc.ModelBinding;
using MiniAppJuanTemplate.Models;

namespace MiniAppJuanTemplate.Areas.Manage.Services.Interfaces
{
    public interface ITagService
    {
        void Create(Tag tag, ModelStateDictionary ModelState);
        void Update(int? id);
        void Delete(int? id);
        void Detail(int? id);
        List<Product> GetAllProducts();
        Product GetProductById();
    }
}

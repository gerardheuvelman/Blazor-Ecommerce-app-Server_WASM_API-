using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CategoryRepository(ApplicationDbContext db, IMapper mapper)
    {
        this._db = db;
        this._mapper = mapper;
    }

    public CategoryDTO Create(CategoryDTO objDTO)
    {
        Category category = _mapper.Map<Category>(objDTO);
        category.CreatedDate = DateTime.Now;
        _db.Categories.Add(category);
        _db.SaveChanges();

        return _mapper.Map<CategoryDTO>(category);
    }

    public int Delete(int id)
    {
        var obj = _db.Categories.FirstOrDefault(c => c.Id == id);
        if (obj is not null) 
        {
            _db.Categories.Remove(obj);
            return _db.SaveChanges();
        }
        return 0;
    }

    public CategoryDTO Get(int id)
    {
        var obj = _db.Categories.FirstOrDefault(c => c.Id == id);
        if (obj is not null)
        {
            return _mapper.Map<CategoryDTO>(obj);
        }
        return new CategoryDTO();
    }

    public IEnumerable<CategoryDTO> GetAll()
    {
        return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(_db.Categories);
    }

    public CategoryDTO Update(CategoryDTO objDTO)
    {
        Category? objFromDb = _db.Categories.FirstOrDefault(u => u.Id == objDTO.Id);
        if (objFromDb is not null) 
        {
            // manual mapping

            objFromDb.Name = objDTO.Name;
            _db.Categories.Update(objFromDb);
            _db.SaveChanges();
            return _mapper.Map<CategoryDTO>(objFromDb);
        }
        return objDTO;
    }
}

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
        _db.Categories.Add(category);
        _db.SaveChanges();

        return _mapper.Map<CategoryDTO>(category);
    }

    public int Delete(int id)
    {
        throw new NotImplementedException();
    }

    public CategoryDTO Get(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<CategoryDTO> GetAll()
    {
        throw new NotImplementedException();
    }

    public CategoryDTO Update(CategoryDTO objDTO)
    {
        throw new NotImplementedException();
    }
}

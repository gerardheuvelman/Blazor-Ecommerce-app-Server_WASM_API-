using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ProductRepository(ApplicationDbContext db, IMapper mapper)
    {
        this._db = db;
        this._mapper = mapper;
    }

    public async Task<ProductDTO> Create(ProductDTO objDTO)
    {
        Product product = _mapper.Map<Product>(objDTO);
        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<int> Delete(int id)
    {
        var obj = await _db.Products.FirstOrDefaultAsync(c => c.Id == id);
        if (obj is not null) 
        {
            _db.Products.Remove(obj);
            return await _db.SaveChangesAsync();
        }
        return 0;
    }

    public async Task<ProductDTO> Get(int id)
    {
        var obj = await _db.Products.FirstOrDefaultAsync(c => c.Id == id);
        if (obj is not null)
        {
            return _mapper.Map<ProductDTO>(obj);
        }
        return new ProductDTO();
    }

    public async Task<IEnumerable<ProductDTO>> GetAll()
    {
        return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(_db.Products);
    }

    public async Task<ProductDTO> Update(ProductDTO objDTO)
    {
        Product? objFromDb = _db.Products.FirstOrDefault(u => u.Id == objDTO.Id);
        if (objFromDb is not null)
        {
            // manual mapping
            objFromDb.Name = objDTO.Name;
            objFromDb.Description = objDTO.Description;
            objFromDb.ShopFavorites = objDTO.ShopFavorites;
            objFromDb.CustomerFavorites = objDTO.CustomerFavorites;
            objFromDb.Color = objDTO.Color;
            objFromDb.ImageUrl = objDTO.ImageUrl;
            objFromDb.CategoryId = objDTO.CategoryId;

            _db.Products.Update(objFromDb);
            await _db.SaveChangesAsync();
            return _mapper.Map<ProductDTO>(objFromDb);
        }
        return objDTO;
    }
}

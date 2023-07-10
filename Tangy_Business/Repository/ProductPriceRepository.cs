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

public class ProductPriceRepository : IProductPriceRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ProductPriceRepository(ApplicationDbContext db, IMapper mapper)
    {
        this._db = db;
        this._mapper = mapper;
    }

    public async Task<ProductPriceDTO> Create(ProductPriceDTO objDTO)
    {
        ProductPrice productPrice = _mapper.Map<ProductPrice>(objDTO);
        _db.ProductPrices.Add(productPrice);
        await _db.SaveChangesAsync();

        return _mapper.Map<ProductPriceDTO>(productPrice);
    }

    public async Task<int> Delete(int id)
    {
        var obj = await _db.ProductPrices.FirstOrDefaultAsync(c => c.Id == id);
        if (obj is not null) 
        {
            _db.ProductPrices.Remove(obj);
            return await _db.SaveChangesAsync();
        }
        return 0;
    }

    public async Task<ProductPriceDTO> Get(int id)
    {
        var obj = await _db.ProductPrices.FirstOrDefaultAsync(c =>c.Id==id);
        if (obj is not null)
        {
            return _mapper.Map<ProductPriceDTO>(obj);
        }
        return new ProductPriceDTO();
    }

    public async Task<IEnumerable<ProductPriceDTO>> GetAll(int? id = null)
    {
        if (id is not null && id > 0)
        {
            return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>(_db.ProductPrices.Where(u => u.ProductId == id));
        }
        else
        { 
            return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>(_db.ProductPrices);
        }
    }

    public async Task<ProductPriceDTO> Update(ProductPriceDTO objDTO)
    {
        ProductPrice? objFromDb = _db.ProductPrices.FirstOrDefault(u => u.Id == objDTO.Id);
        if (objFromDb is not null)
        {
            // manual mapping
            objFromDb.ProductId = objDTO.ProductId;
            objFromDb.Size = objDTO.Size;
            objFromDb.Price = objDTO.Price;
            _db.ProductPrices.Update(objFromDb);
            await _db.SaveChangesAsync();
            return _mapper.Map<ProductPriceDTO>(objFromDb);
        }
        return objDTO;
    }
}

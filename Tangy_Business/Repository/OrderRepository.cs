﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_DataAccess.ViewModel;
using Tangy_Models;

namespace Tangy_Business.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public OrderRepository(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public Task<OrderHeaderDTO> CancelOrder(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderDTO> Create(OrderDTO objDTO)
    {
        try
        {
            var obj = _mapper.Map<OrderDTO, Order>(objDTO);
            _db.OrderHeaders.Add(obj.OrderHeader);
            await _db.SaveChangesAsync();

            foreach (var detail in obj.OrderDetails)
            {
                detail.OrderHeaderId = obj.OrderHeader.Id;
            }
            _db.OrderDetails.AddRange(obj.OrderDetails);
            await _db.SaveChangesAsync();

            return new OrderDTO
            {
                OrderHeader = _mapper.Map<OrderHeader, OrderHeaderDTO>(obj.OrderHeader),
                OrderDetails = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDTO>> (obj.OrderDetails).ToList()
            };
        }
        catch (Exception ex)
        {

            throw ex;
        }

        return objDTO;
    }

    public async Task<int> Delete(int id)
    {
        var objHeader = await _db.OrderHeaders.FirstOrDefaultAsync(u => u.Id == id);
        if (objHeader is not null)
        {
            IEnumerable<OrderDetail> objDetails = _db.OrderDetails.Where(u => u.OrderHeaderId == id);
            _db.OrderDetails.RemoveRange(objDetails);
            _db.OrderHeaders.Remove(objHeader);
            return _db.SaveChanges();
        }
        return 0;
    }

    public async Task<OrderDTO> Get(int id)
    {
        Order order = new()
        {
            OrderHeader = _db.OrderHeaders.FirstOrDefault(o => o.Id == id),
            OrderDetails = _db.OrderDetails.Where(o => o.OrderHeaderId == id)
        };
        if (order is not null)
        {
            return _mapper.Map<Order, OrderDTO>(order);
        }
        return new OrderDTO();
    }

    public async Task<IEnumerable<OrderDTO>> GetAll(string? userId = null, string? status = null)
    {
        List<Order> ordersFromDb = new List<Order>();
        IEnumerable<OrderHeader> orderHeaderList = _db.OrderHeaders;
        IEnumerable<OrderDetail> orderDetailsList = _db.OrderDetails;

        Order order;

        foreach (OrderHeader header in orderHeaderList)
        {
            IEnumerable<OrderDetail> detailsForOrderHeader = orderDetailsList.Where(od=>od.OrderHeaderId == header.Id);
            
            order = new Order()
            {
                OrderHeader = header,
                OrderDetails = detailsForOrderHeader
            };
            ordersFromDb.Add(order);
        }
        // Do some filtering : #TODO

        return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(ordersFromDb);

    }

    public async Task<OrderHeaderDTO> MarkPaymentSuccessful(int id)
    {
        var data = await _db.OrderHeaders.FindAsync(id);
        if (data is null) 
        {
            return new OrderHeaderDTO();
        }
        if (data.Status == SD.Status_Pending)
        {
            data.Status = SD.Status_Confirmed;
            await _db.SaveChangesAsync();
            return _mapper.Map<OrderHeader, OrderHeaderDTO>(data);
        }
        return new OrderHeaderDTO();
    }

    public async Task<OrderDTO> Update(OrderDTO objDTO)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderHeaderDTO> UpdateHeader(OrderHeaderDTO objDTO)
    {
        if (objDTO is not null)
        {
            var orderHeader = _mapper.Map<OrderHeaderDTO, OrderHeader>(objDTO);
            _db.OrderHeaders.Update(orderHeader);
            await _db.SaveChangesAsync();
            return _mapper.Map<OrderHeader, OrderHeaderDTO>(orderHeader);
        }
        return new OrderHeaderDTO();
    }

    public async Task<bool> UpdateOrderStatus(int orderId, string status)
    {
        var data = await _db.OrderHeaders.FindAsync(orderId);
        if (data is null)
        {
            return false;
        }
        data.Status = status;
        if (status == SD.Status_Shipped)
        {
            data.OrderDate = DateTime.Now;
        }
        await _db.SaveChangesAsync();
        return true;
    }

    private async Task HandleCheckout()
    {

    }
}

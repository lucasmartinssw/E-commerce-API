using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Ecommerce.Application.UseCases.Products.GetAllPaged;

public class GetAllPagedProductsUseCase
{
    private readonly IProductRepository _repository;

    public GetAllPagedProductsUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponsePagedData<ResponseProductSummaryJson>> Execute(RequestProductQuery query)
    {
        var pagedResult = await _repository.GetAllPaged(
            query.PageNumber,
            query.PageSize,
            query.SortBy,
            query.CategoryId,
            query.MinPrice,
            query.MaxPrice,
            query.SearchTerm
        );

        var productResponses = pagedResult.Items.Select(p => new ResponseProductSummaryJson
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            ImageFilename = p.ImageFilename,
            CategoryName = p.Category.Name
        }).ToList();

    
        var totalPages = (int)Math.Ceiling(pagedResult.TotalCount / (double)query.PageSize);

        
        return new ResponsePagedData<ResponseProductSummaryJson>
        {
            Data = productResponses,
            TotalCount = pagedResult.TotalCount,
            CurrentPage = query.PageNumber,
            PageSize = query.PageSize,
            TotalPages = totalPages
        };
    }
}
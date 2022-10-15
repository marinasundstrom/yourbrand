﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Identity;
using YourBrand.Marketing.Domain.Entities;
using YourBrand.Marketing.Domain;

namespace YourBrand.Marketing.Application.Discounts.Queries;

public record GetDiscountsQuery(int Page = 0, int PageSize = 10, string? SearchString = null, string? SortBy = null, Application.Common.Models.SortDirection? SortDirection = null) : IRequest<ItemsResult<DiscountDto>>
{
    class GetDiscountsQueryHandler : IRequestHandler<GetDiscountsQuery, ItemsResult<DiscountDto>>
    {
        private readonly IMarketingContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetDiscountsQueryHandler(
            IMarketingContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<ItemsResult<DiscountDto>> Handle(GetDiscountsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Discount> result = _context
                    .Discounts
                    .OrderBy(o => o.Created)
                    .AsNoTracking()
                    .AsQueryable();

            if (request.SearchString is not null)
            {
                result = result.Where(o => o.ProductName.ToLower().Contains(request.SearchString.ToLower()));
            }

            var totalCount = await result.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                result = result.OrderBy(request.SortBy, request.SortDirection == Application.Common.Models.SortDirection.Desc ? Marketing.Application.SortDirection.Descending : Marketing.Application.SortDirection.Ascending);
            }
            else 
            {
                result = result.OrderBy(x => x.Id);
            }

            var items = await result
                .Skip((request.Page) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            return new ItemsResult<DiscountDto>(items.Select(cp => cp.ToDto()), totalCount);
        }
    }
}

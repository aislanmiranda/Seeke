﻿using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default);

    Task<List<Product>> GetByIdsAsync(IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);
}
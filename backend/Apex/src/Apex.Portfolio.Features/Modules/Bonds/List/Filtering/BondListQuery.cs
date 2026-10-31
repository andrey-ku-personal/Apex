using System.Linq.Expressions;
using Apex.Shared.Core.Pagination;
using Microsoft.EntityFrameworkCore;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.Bonds.List.Filtering;

public class BondListQuery : BaseSortQuery<BondEntity>
{
    public int? CurrencyId { get; set; }
    public int? PaymentFrequencyId { get; set; }
    public string? FreeText { get; set; }

    public override Expression<Func<BondEntity, bool>> GetExpression()
        => bond =>
            (CurrencyId == null || bond.CurrencyId == CurrencyId) &&
            (PaymentFrequencyId == null || bond.PaymentFrequencyId == PaymentFrequencyId) &&
            (string.IsNullOrWhiteSpace(FreeText) ||
             EF.Functions.ILike(bond.Ticker, $"%{FreeText}%") ||
             EF.Functions.ILike(bond.Issuer, $"%{FreeText}%"));
}
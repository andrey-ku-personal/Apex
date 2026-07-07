using Apex.Invest.Abstracts.List;
using Apex.Invest.Domain.Entities;
using Apex.Invest.Features.Modules.Bonds.List.Filters;
using Apex.Invest.Features.Modules.Bonds.List.Models;
using Apex.Invest.Features.Modules.Bonds.List.Queries;

namespace Apex.Invest.Features.Modules.Bonds.List.Services;

public interface IBondListService : IBaseListService<BondListModel, Bond, BondListFilter, BondListQuery>;

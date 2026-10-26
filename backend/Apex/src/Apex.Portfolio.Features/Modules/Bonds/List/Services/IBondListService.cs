using Apex.Portfolio.Abstracts.List;
using Apex.Portfolio.Features.Modules.Bonds.List.Filtering;
using Apex.Portfolio.Features.Modules.Bonds.List.Models;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.Bonds.List.Services;

public interface IBondListService : IBaseListService<BondListModel, BondEntity, BondListFilter, BondListQuery>;
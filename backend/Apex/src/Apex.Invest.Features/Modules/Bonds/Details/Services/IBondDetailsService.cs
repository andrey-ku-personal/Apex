using Apex.Invest.Abstracts.Details;
using Apex.Invest.Domain.Entities.Bond;
using Apex.Invest.Features.Modules.Bonds.Details.Filters;
using Apex.Invest.Features.Modules.Bonds.Details.Models;

namespace Apex.Invest.Features.Modules.Bonds.Details.Services;

public interface IBondDetailsService : IBaseDetailsService<BondDetailsModel, Bond, BondDetailsFilter>;

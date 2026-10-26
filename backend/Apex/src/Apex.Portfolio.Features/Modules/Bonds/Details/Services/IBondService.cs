using Apex.Portfolio.Abstracts.Details;
using Apex.Portfolio.Features.Modules.Bonds.Details.Filtering;
using Apex.Portfolio.Features.Modules.Bonds.Details.Models;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.Bonds.Details.Services;

public interface IBondService : IBaseDetailsService<BondModel, BondEntity, BondFilter>;
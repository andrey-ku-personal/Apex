using Apex.Invest.Domain.Entities.Bond;
using Apex.Invest.Features.Modules.Bonds.Details.Filters;
using Apex.Invest.Features.Modules.Bonds.Details.Models;
using Apex.Invest.Features.Modules.Bonds.Details.Queries;
using Apex.Invest.Modules;
using Apex.Invest.Services;

namespace Apex.Invest.Features.Modules.Bonds.Details.Services;

public class BondDetailsDecorator(
    BondDetailsService service,
    IValidationRunner validator
) : BaseManageServiceDecorator<BondDetailsModel, Bond, BondDetailsFilter, BondDetailsQuery>(service, validator), IBondDetailsService;

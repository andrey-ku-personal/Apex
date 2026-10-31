using Apex.Portfolio.Features.Modules.Bonds.Details.Filtering;
using Apex.Portfolio.Features.Modules.Bonds.Details.Models;
using Apex.Portfolio.Modules;
using Apex.Shared.Core.Services;
using BondEntity = Apex.Portfolio.Domain.Entities.Bond.Bond;

namespace Apex.Portfolio.Features.Modules.Bonds.Details.Services;

public class BondDecorator(
    BondService service,
    IValidationRunner validator
) : BaseManageServiceDecorator<BondModel, BondEntity, BondFilter, BondQuery>(service, validator), IBondService;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Filtering;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Models;
using Apex.Portfolio.Modules;
using Apex.Shared.Core.Services;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;

namespace Apex.Portfolio.Features.Modules.BondOperations.Details.Services;

public class BondOperationDecorator(
    BondOperationService service,
    IValidationRunner validator
) : BaseManageServiceDecorator<BondOperationModel, BondOperationEntity, BondOperationFilter, BondOperationQuery>(service, validator), IBondOperationService;

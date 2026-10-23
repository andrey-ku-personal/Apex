using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Filtering;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Models;
using Apex.Portfolio.Modules;
using Apex.Portfolio.Services;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Services;

public class FinancialAttributeDecorator(
    FinancialAttributeService service,
    IValidationRunner validator
) : BaseManageServiceDecorator<
        FinancialAttributeModel, 
        Domain.Abstractions.FinancialAttribute, 
        FinancialAttributeFilter, 
        FinancialAttributeQuery
    >(service, validator), IFinancialAttributeService;

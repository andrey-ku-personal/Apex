using Apex.Portfolio.Abstracts.Details;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Filters;
using Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Models;

namespace Apex.Portfolio.Features.Modules.FinancialAttribute.Details.Services;

public interface IFinancialAttributeService : IBaseDetailsService<FinancialAttributeModel, Domain.Abstractions.FinancialAttribute, FinancialAttributeFilter>;

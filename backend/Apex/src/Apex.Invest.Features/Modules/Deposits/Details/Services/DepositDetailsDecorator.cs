using Apex.Invest.Domain.Entities;
using Apex.Invest.Features.Modules.Deposits.Details.Filters;
using Apex.Invest.Features.Modules.Deposits.Details.Models;
using Apex.Invest.Features.Modules.Deposits.Details.Queries;
using Apex.Invest.Modules;
using Apex.Invest.Services;

namespace Apex.Invest.Features.Modules.Deposits.Details.Services;

public class DepositDetailsDecorator(
    DepositDetailsService service,
    IValidationRunner validator
) : BaseManageServiceDecorator<DepositDetailsModel, Deposit, DepositDetailsFilter, DepositDetailsQuery>(service, validator), IDepositDetailsService;

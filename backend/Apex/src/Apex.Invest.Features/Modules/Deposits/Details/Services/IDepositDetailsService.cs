using Apex.Invest.Abstracts.Details;
using Apex.Invest.Domain.Entities;
using Apex.Invest.Features.Modules.Deposits.Details.Filters;
using Apex.Invest.Features.Modules.Deposits.Details.Models;

namespace Apex.Invest.Features.Modules.Deposits.Details.Services;

public interface IDepositDetailsService : IBaseDetailsService<DepositDetailsModel, Deposit, DepositDetailsFilter>;

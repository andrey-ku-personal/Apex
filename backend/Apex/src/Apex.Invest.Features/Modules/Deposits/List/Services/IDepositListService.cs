using Apex.Invest.Abstracts.List;
using Apex.Invest.Domain.Entities;
using Apex.Invest.Features.Modules.Deposits.List.Filters;
using Apex.Invest.Features.Modules.Deposits.List.Models;
using Apex.Invest.Features.Modules.Deposits.List.Queries;

namespace Apex.Invest.Features.Modules.Deposits.List.Services;

public interface IDepositListService : IBaseListService<DepositListModel, Deposit, DepositListFilter, DepositListQuery>;

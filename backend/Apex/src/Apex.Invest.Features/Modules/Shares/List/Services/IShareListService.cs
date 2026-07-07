using Apex.Invest.Abstracts.List;
using Apex.Invest.Domain.Entities;
using Apex.Invest.Features.Modules.Shares.List.Filters;
using Apex.Invest.Features.Modules.Shares.List.Models;
using Apex.Invest.Features.Modules.Shares.List.Queries;

namespace Apex.Invest.Features.Modules.Shares.List.Services;

public interface IShareListService : IBaseListService<ShareListModel, Share, ShareListFilter, ShareListQuery>;

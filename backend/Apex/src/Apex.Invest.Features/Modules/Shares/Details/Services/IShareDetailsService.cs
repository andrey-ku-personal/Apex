using Apex.Invest.Abstracts.Details;
using Apex.Invest.Domain.Entities;
using Apex.Invest.Features.Modules.Shares.Details.Filters;
using Apex.Invest.Features.Modules.Shares.Details.Models;

namespace Apex.Invest.Features.Modules.Shares.Details.Services;

public interface IShareDetailsService : IBaseDetailsService<ShareDetailsModel, Share, ShareDetailsFilter>;

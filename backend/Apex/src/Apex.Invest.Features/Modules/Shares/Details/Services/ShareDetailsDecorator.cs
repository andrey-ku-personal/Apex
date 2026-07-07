using Apex.Invest.Domain.Entities;
using Apex.Invest.Features.Modules.Shares.Details.Filters;
using Apex.Invest.Features.Modules.Shares.Details.Models;
using Apex.Invest.Features.Modules.Shares.Details.Queries;
using Apex.Invest.Modules;
using Apex.Invest.Services;

namespace Apex.Invest.Features.Modules.Shares.Details.Services;

public class ShareDetailsDecorator(
    ShareDetailsService service,
    IValidationRunner validator
) : BaseManageServiceDecorator<ShareDetailsModel, Share, ShareDetailsFilter, ShareDetailsQuery>(service, validator), IShareDetailsService;

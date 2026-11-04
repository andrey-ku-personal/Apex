using Apex.Portfolio.Abstracts.Details;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Filtering;
using Apex.Portfolio.Features.Modules.BondOperations.Details.Models;
using BondOperationEntity = Apex.Portfolio.Domain.Entities.Bond.BondOperation;

namespace Apex.Portfolio.Features.Modules.BondOperations.Details.Services;

public interface IBondOperationService : IBaseDetailsService<BondOperationModel, BondOperationEntity, BondOperationFilter>;

using Shouldly;
using Apex.Invest.Features.Modules.Deposits.Details.Filters;
using Apex.Invest.Features.Modules.Deposits.Details.Models;
using Apex.Invest.Features.Modules.Deposits.Details.Services;
using Apex.Invest.Features.Modules.Deposits.Details.Validators;
using Apex.Shared.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Invest.Features.Tests.Modules.Deposits.Details;

[Collection(nameof(SliceFixture))]
public class DepositDetailsTests(SliceFixture fixture)
{
    private readonly SliceFixture _fixture = fixture;

    [Fact]
    public void Invalid_Model_Is_Not_Valid()
    {
        var validator = new DepositDetailsModelValidator();

        var cases = new (DepositDetailsModel Model, string Property)[]
        {
            (MakeInvalid(m => m.Ticker = ""), nameof(DepositDetailsModel.Ticker)),
            (MakeInvalid(m => m.Ticker = null!), nameof(DepositDetailsModel.Ticker)),
            (MakeInvalid(m => m.Issuer = ""), nameof(DepositDetailsModel.Issuer)),
            (MakeInvalid(m => m.Issuer = null!), nameof(DepositDetailsModel.Issuer)),
            (MakeInvalid(m => m.PlatformId = 0), nameof(DepositDetailsModel.PlatformId))
        };

        foreach (var (model, property) in cases)
        {
            var result = validator.Validate(model);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.PropertyName == property);
        }

        DepositDetailsModel MakeInvalid(Action<DepositDetailsModel> change)
        {
            var model = new DepositDetailsFaker().FakeModel(_fixture.Platforms);
            change(model);
            return model;
        }
    }

    [Fact]
    public void Invalid_FindFilter_Is_Not_Valid()
    {
        var validator = new DepositDetailsFilterValidator();

        var cases = new (DepositDetailsFilter Model, string Property)[]
        {
            (MakeInvalid(m => m.Id = 0), nameof(DepositDetailsFilter.Id))
        };

        foreach (var (model, property) in cases)
        {
            var result = validator.Validate(model);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.PropertyName == property);
        }

        DepositDetailsFilter MakeInvalid(Action<DepositDetailsFilter> change)
        {
            var m = new DepositDetailsFilter() { Id = 1 };
            change(m);
            return m;
        }
    }

    [Fact]
    public async Task Create_Deposit()
    {
        await _fixture.InitializeAsync();

        var model = new DepositDetailsFaker().FakeModel(_fixture.Platforms);
        var result = await _fixture.UseServiceAsync<IDepositDetailsService, DepositDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        result.Id.ShouldBeGreaterThan(0);
        IsEqual(model, result);
    }

    [Fact]
    public async Task Update_Deposit()
    {
        await _fixture.InitializeAsync();

        var model = new DepositDetailsFaker().FakeModel(_fixture.Platforms);
        var result = await _fixture.UseServiceAsync<IDepositDetailsService, DepositDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        result.Id.ShouldBeGreaterThan(0);

        model = new DepositDetailsFaker().FakeModel(_fixture.Platforms, result.Id);
        model.Ticker = $"UPDATED_{Guid.NewGuid():N}";
        model.Capitalization = !result.Capitalization;
        result = await _fixture.UseServiceAsync<IDepositDetailsService, DepositDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        result.Id.ShouldBe(model.Id);
        result.Ticker.ShouldBe(model.Ticker);
        result.Capitalization.ShouldBe(model.Capitalization);
    }

    [Fact]
    public async Task Get_After_Create()
    {
        await _fixture.InitializeAsync();

        var model = new DepositDetailsFaker().FakeModel(_fixture.Platforms);
        var created = await _fixture.UseServiceAsync<IDepositDetailsService, DepositDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        var fetched = await _fixture.UseServiceAsync<IDepositDetailsService, DepositDetailsModel>(
            svc => svc.Get(new DepositDetailsFilter { Id = created.Id }, CancellationToken.None));

        fetched.ShouldNotBeNull();
        fetched.Id.ShouldBe(created.Id);
        fetched.Ticker.ShouldBe(created.Ticker);
        fetched.Issuer.ShouldBe(created.Issuer);
    }

    [Fact]
    public async Task Get_NotFound_Should_Throw()
    {
        await _fixture.InitializeAsync();

        var exception = await Should.ThrowAsync<NotFoundException>(
            () => _fixture.UseServiceAsync<IDepositDetailsService, DepositDetailsModel>(
                svc => svc.Get(new DepositDetailsFilter { Id = 999 }, CancellationToken.None)));

        exception.Message.ShouldContain("Deposit was not found");
    }

    private static void IsEqual(DepositDetailsModel model, DepositDetailsModel result)
    {
        result.Ticker.ShouldBe(model.Ticker);
        result.Issuer.ShouldBe(model.Issuer);
        result.InterestRate.ShouldBe(model.InterestRate);
        result.Frequence.ShouldBe(model.Frequence);
        result.IsReplenishable.ShouldBe(model.IsReplenishable);
        result.IsTaxFree.ShouldBe(model.IsTaxFree);
        result.Capitalization.ShouldBe(model.Capitalization);
    }
}

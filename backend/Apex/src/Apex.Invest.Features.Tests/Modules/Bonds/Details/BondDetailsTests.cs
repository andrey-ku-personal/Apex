using Shouldly;
using Apex.Invest.Features.Modules.Bonds.Details.Filters;
using Apex.Invest.Features.Modules.Bonds.Details.Models;
using Apex.Invest.Features.Modules.Bonds.Details.Services;
using Apex.Invest.Features.Modules.Bonds.Details.Validators;
using Apex.Shared.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Apex.Invest.Domain.Entities;

namespace Apex.Invest.Features.Tests.Modules.Bonds.Details;

[Collection(nameof(SliceFixture))]
public class BondDetailsTests(SliceFixture fixture)
{
    private readonly SliceFixture _fixture = fixture;

    [Fact]
    public void Invalid_Model_Is_Not_Valid()
    {
        var validator = new BondDetailsModelValidator();

        var cases = new (BondDetailsModel Model, string Property)[]
        {
            (MakeInvalid(m => m.Ticker = ""), nameof(BondDetailsModel.Ticker)),
            (MakeInvalid(m => m.Ticker = null!), nameof(BondDetailsModel.Ticker)),
            (MakeInvalid(m => m.Issuer = ""), nameof(BondDetailsModel.Issuer)),
            (MakeInvalid(m => m.Issuer = null!), nameof(BondDetailsModel.Issuer)),
            (MakeInvalid(m => m.PlatformId = 0), nameof(BondDetailsModel.PlatformId)),
            (MakeInvalid(m => m.NominalPrice = 0), nameof(BondDetailsModel.NominalPrice)),
            (MakeInvalid(m => m.Quantity = 0), nameof(BondDetailsModel.Quantity))
        };

        foreach (var (model, property) in cases)
        {
            var result = validator.Validate(model);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.PropertyName == property);
        }

        BondDetailsModel MakeInvalid(Action<BondDetailsModel> change)
        {
            var model = new BondDetailsFaker().FakeModel(_fixture.Platforms);
            change(model);
            return model;
        }
    }

    [Fact]
    public void Invalid_FindFilter_Is_Not_Valid()
    {
        var validator = new BondDetailsFilterValidator();

        var cases = new (BondDetailsFilter Model, string Property)[]
        {
            (MakeInvalid(m => m.Id = 0), nameof(BondDetailsFilter.Id))
        };

        foreach (var (model, property) in cases)
        {
            var result = validator.Validate(model);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.PropertyName == property);
        }

        BondDetailsFilter MakeInvalid(Action<BondDetailsFilter> change)
        {
            var m = new BondDetailsFilter() { Id = 1 };
            change(m);
            return m;
        }
    }

    [Fact]
    public async Task Create_Bond()
    {
        await _fixture.InitializeAsync();

        var model = new BondDetailsFaker().FakeModel(_fixture.Platforms);

        var result = await _fixture.UseServiceAsync<IBondDetailsService, BondDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        result.Id.ShouldBeGreaterThan(0);
        IsEqual(model, result);
    }

    [Fact]
    public async Task Update_Bond()
    {
        await _fixture.InitializeAsync();

        var model = new BondDetailsFaker().FakeModel(_fixture.Platforms);

        var result = await _fixture.UseServiceAsync<IBondDetailsService, BondDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        result.Id.ShouldBeGreaterThan(0);

        model = new BondDetailsFaker().FakeModel(_fixture.Platforms, result.Id);
        model.Ticker = $"UPDATED_{Guid.NewGuid():N}";
        result = await _fixture.UseServiceAsync<IBondDetailsService, BondDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        result.Id.ShouldBe(model.Id);
        result.Ticker.ShouldBe(model.Ticker);
    }

    [Fact]
    public async Task Get_After_Create()
    {
        await _fixture.InitializeAsync();

        var model = new BondDetailsFaker().FakeModel(_fixture.Platforms);

        var created = await _fixture.UseServiceAsync<IBondDetailsService, BondDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        var fetched = await _fixture.UseServiceAsync<IBondDetailsService, BondDetailsModel>(
            svc => svc.Get(new BondDetailsFilter { Id = created.Id }, CancellationToken.None));

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
            () => _fixture.UseServiceAsync<IBondDetailsService, BondDetailsModel>(
                svc => svc.Get(new BondDetailsFilter { Id = 999 }, CancellationToken.None)));

        exception.Message.ShouldContain("Bond was not found");
    }

    private static void IsEqual(BondDetailsModel model, BondDetailsModel result)
    {
        result.Ticker.ShouldBe(model.Ticker);
        result.Issuer.ShouldBe(model.Issuer);
        result.InterestRate.ShouldBe(model.InterestRate);
        result.Currency.ShouldBe(model.Currency);
        result.NominalPrice.ShouldBe(model.NominalPrice);
        result.Quantity.ShouldBe(model.Quantity);
    }
}

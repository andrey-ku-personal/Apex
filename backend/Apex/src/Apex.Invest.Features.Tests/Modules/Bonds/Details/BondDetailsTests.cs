using Shouldly;
using Apex.Invest.Features.Modules.Bonds.Details.Filters;
using Apex.Invest.Features.Modules.Bonds.Details.Models;
using Apex.Invest.Features.Modules.Bonds.Details.Services;
using Apex.Invest.Features.Modules.Bonds.Details.Validators;
using Apex.Shared.Core.Exceptions;
using Apex.Invest.Features.Tests.Modules.Bonds.Details.Fakers;
using Apex.Shared.Core.Extensions;

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
            (MakeInvalid(m => m.ParPrice = 0), nameof(BondDetailsModel.ParPrice)),
            (MakeInvalid(m => m.NextCouponDate = 0), nameof(BondDetailsModel.ParPrice))
        };

        foreach (var (model, property) in cases)
        {
            var result = validator.Validate(model);
            result.IsValid.ShouldBeFalse();
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

        var result = await UpsertData(model);

        IsEqual(model, result);
    }


    [Fact]
    public async Task Get_Bond()
    {
        await _fixture.InitializeAsync();

        var model = new BondDetailsFaker().FakeModel(_fixture.Platforms);

        var result = await UpsertData(model);

        result = await GetData(result.Id);

        IsEqual(model, result);
    }

    [Fact]
    public async Task Update_Bond()
    {
        await _fixture.InitializeAsync();

        var model = new BondDetailsFaker().FakeModel(_fixture.Platforms);

        var result = await UpsertData(model);

        result.Id.ShouldBeGreaterThan(0);

        model = new BondDetailsFaker().FakeModel(_fixture.Platforms, result.Id);
        model.Ticker = $"UPDATED_{Guid.NewGuid():N}";
        model.Operations =  [..result.Operations!.Take(10), ..model.Operations!];

        result = await UpsertData(model);

        IsEqual(model, result);
    }

    private async Task<BondDetailsModel> UpsertData(BondDetailsModel model) =>
        await _fixture.UseServiceAsync<IBondDetailsService, BondDetailsModel>(svc => svc.Upsert(model, new CancellationToken()));

    private async Task<BondDetailsModel> GetData(int id) =>
        await _fixture.UseServiceAsync<IBondDetailsService, BondDetailsModel>(svc => svc.Get(new() { Id = id }, new CancellationToken()));

    private static void IsEqual(BondDetailsModel model, BondDetailsModel result)
    {
        result.Id.ShouldBeGreaterThan(0);
        result.Ticker.ShouldBe(model.Ticker);
        result.Issuer.ShouldBe(model.Issuer);
        result.Currency.ShouldBe(model.Currency);
        result.ParPrice.ShouldBe(model.ParPrice);
        result.CouponRate.ShouldBe(model.CouponRate);
        result.PaymentFrequence.ShouldBe(model.PaymentFrequence);
        result.MaturityDate.ShouldBe(model.MaturityDate);
        if (result.Operations.HasAny())
            result.Operations!.Count.ShouldBe(model.Operations!.Count);

        result.Operations?.ForEach(operation => 
            model.Operations!.Any(o => o.Type == operation.Type && o.Price == operation.Price && o.Count == operation.Count).ShouldBeTrue());
    }
}
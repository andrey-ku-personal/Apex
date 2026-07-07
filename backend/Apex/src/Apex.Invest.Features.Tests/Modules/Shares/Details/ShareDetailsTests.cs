using Shouldly;
using Apex.Invest.Features.Modules.Shares.Details.Filters;
using Apex.Invest.Features.Modules.Shares.Details.Models;
using Apex.Invest.Features.Modules.Shares.Details.Services;
using Apex.Invest.Features.Modules.Shares.Details.Validators;
using Apex.Shared.Core.Exceptions;

namespace Apex.Invest.Features.Tests.Modules.Shares.Details;

[Collection(nameof(SliceFixture))]
public class ShareDetailsTests(SliceFixture fixture)
{
    private readonly SliceFixture _fixture = fixture;

    [Fact]
    public void Invalid_Model_Is_Not_Valid()
    {
        var validator = new ShareDetailsModelValidator();

        var cases = new (ShareDetailsModel Model, string Property)[]
        {
            (MakeInvalid(m => m.Ticker = ""), nameof(ShareDetailsModel.Ticker)),
            (MakeInvalid(m => m.Ticker = null!), nameof(ShareDetailsModel.Ticker)),
            (MakeInvalid(m => m.Issuer = ""), nameof(ShareDetailsModel.Issuer)),
            (MakeInvalid(m => m.Issuer = null!), nameof(ShareDetailsModel.Issuer)),
            (MakeInvalid(m => m.PlatformId = 0), nameof(ShareDetailsModel.PlatformId)),
            (MakeInvalid(m => m.MarketPrice = 0), nameof(ShareDetailsModel.MarketPrice)),
            (MakeInvalid(m => m.Quantity = 0), nameof(ShareDetailsModel.Quantity))
        };

        foreach (var (model, property) in cases)
        {
            var result = validator.Validate(model);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.PropertyName == property);
        }

        ShareDetailsModel MakeInvalid(Action<ShareDetailsModel> change)
        {
            var model = new ShareDetailsFaker().FakeModel(_fixture.Platforms);
            change(model);
            return model;
        }
    }

    [Fact]
    public void Invalid_FindFilter_Is_Not_Valid()
    {
        var validator = new ShareDetailsFilterValidator();

        var cases = new (ShareDetailsFilter Model, string Property)[]
        {
            (MakeInvalid(m => m.Id = 0), nameof(ShareDetailsFilter.Id))
        };

        foreach (var (model, property) in cases)
        {
            var result = validator.Validate(model);
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.PropertyName == property);
        }

        ShareDetailsFilter MakeInvalid(Action<ShareDetailsFilter> change)
        {
            var m = new ShareDetailsFilter() { Id = 1 };
            change(m);
            return m;
        }
    }

    [Fact]
    public async Task Create_Share()
    {
        await _fixture.InitializeAsync();

        var model = new ShareDetailsFaker().FakeModel(_fixture.Platforms);

        var result = await _fixture.UseServiceAsync<IShareDetailsService, ShareDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        result.Id.ShouldBeGreaterThan(0);
        IsEqual(model, result);
    }

    [Fact]
    public async Task Update_Share()
    {
        await _fixture.InitializeAsync();

        var model = new ShareDetailsFaker().FakeModel(_fixture.Platforms);

        var result = await _fixture.UseServiceAsync<IShareDetailsService, ShareDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        result.Id.ShouldBeGreaterThan(0);

        model = new ShareDetailsFaker().FakeModel(_fixture.Platforms, result.Id);
        model.Ticker = $"UPDATED_{Guid.NewGuid():N}";
        result = await _fixture.UseServiceAsync<IShareDetailsService, ShareDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        result.Id.ShouldBe(model.Id);
        result.Ticker.ShouldBe(model.Ticker);
    }

    [Fact]
    public async Task Get_After_Create()
    {
        await _fixture.InitializeAsync();

        var model = new ShareDetailsFaker().FakeModel(_fixture.Platforms);

        var created = await _fixture.UseServiceAsync<IShareDetailsService, ShareDetailsModel>(
            svc => svc.Upsert(model, CancellationToken.None));

        var fetched = await _fixture.UseServiceAsync<IShareDetailsService, ShareDetailsModel>(
            svc => svc.Get(new ShareDetailsFilter { Id = created.Id }, CancellationToken.None));

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
            () => _fixture.UseServiceAsync<IShareDetailsService, ShareDetailsModel>(
                svc => svc.Get(new ShareDetailsFilter { Id = 999 }, CancellationToken.None)));

        exception.Message.ShouldContain("Share was not found");
    }

    private static void IsEqual(ShareDetailsModel model, ShareDetailsModel result)
    {
        result.Ticker.ShouldBe(model.Ticker);
        result.Issuer.ShouldBe(model.Issuer);
        result.Currency.ShouldBe(model.Currency);
        result.MarketPrice.ShouldBe(model.MarketPrice);
        result.Quantity.ShouldBe(model.Quantity);
        result.DividendRate.ShouldBe(model.DividendRate);
    }
}

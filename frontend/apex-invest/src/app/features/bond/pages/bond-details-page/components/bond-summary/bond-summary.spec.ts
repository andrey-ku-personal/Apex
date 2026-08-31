import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BondSummary } from './bond-summary';

describe('BondSummary', () => {
  let component: BondSummary;
  let fixture: ComponentFixture<BondSummary>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BondSummary]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BondSummary);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

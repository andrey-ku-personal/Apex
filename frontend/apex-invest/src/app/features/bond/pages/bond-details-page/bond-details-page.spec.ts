import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BondDetailsPage } from './bond-details-page';

describe('BondDetailsPage', () => {
  let component: BondDetailsPage;
  let fixture: ComponentFixture<BondDetailsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BondDetailsPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BondDetailsPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
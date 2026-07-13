import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BondPage } from './bond-page';

describe('BondPage', () => {
  let component: BondPage;
  let fixture: ComponentFixture<BondPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BondPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BondPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

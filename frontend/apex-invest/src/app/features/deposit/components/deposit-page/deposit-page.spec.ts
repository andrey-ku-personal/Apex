import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DepositPage } from './deposit-page';

describe('DepositPage', () => {
  let component: DepositPage;
  let fixture: ComponentFixture<DepositPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DepositPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DepositPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

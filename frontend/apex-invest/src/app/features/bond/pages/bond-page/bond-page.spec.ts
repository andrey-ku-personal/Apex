import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';

import { BondPage } from './bond-page';
import { BondListApiService } from './services/bond-list-api.service';

describe('BondPage', () => {
  let component: BondPage;
  let fixture: ComponentFixture<BondPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BondPage],
      providers: [
        provideRouter([]),
        { provide: BondListApiService, useValue: { getList: () => of({ totalCount: 0, data: [] }) } },
      ],
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
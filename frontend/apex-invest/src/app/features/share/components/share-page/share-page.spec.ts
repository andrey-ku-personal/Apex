import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharePage } from './share-page';

describe('SharePage', () => {
  let component: SharePage;
  let fixture: ComponentFixture<SharePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SharePage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SharePage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

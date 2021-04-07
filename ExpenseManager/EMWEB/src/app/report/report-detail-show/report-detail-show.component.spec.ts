import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportDetailShowComponent } from './report-detail-show.component';

describe('ReportDetailShowComponent', () => {
  let component: ReportDetailShowComponent;
  let fixture: ComponentFixture<ReportDetailShowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportDetailShowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportDetailShowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

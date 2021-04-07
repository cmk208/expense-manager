import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TransactionAddEditComponent } from './transaction-add-edit.component';

describe('TransactionAddEditComponent', () => {
  let component: TransactionAddEditComponent;
  let fixture: ComponentFixture<TransactionAddEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TransactionAddEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TransactionAddEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

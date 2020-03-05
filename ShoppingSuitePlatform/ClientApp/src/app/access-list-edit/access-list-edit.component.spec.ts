import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccessListEditComponent } from './access-list-edit.component';

describe('AccessListEditComponent', () => {
  let component: AccessListEditComponent;
  let fixture: ComponentFixture<AccessListEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccessListEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccessListEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

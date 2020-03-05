import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccessListsComponent } from './access-lists.component';

describe('AccessListsComponent', () => {
  let component: AccessListsComponent;
  let fixture: ComponentFixture<AccessListsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccessListsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccessListsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

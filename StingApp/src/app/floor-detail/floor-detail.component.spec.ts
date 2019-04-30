import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FloorDetailComponent } from './floor-detail.component';

describe('FloorComponent', () => {
  let component: FloorDetailComponent;
  let fixture: ComponentFixture<FloorDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FloorDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FloorDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

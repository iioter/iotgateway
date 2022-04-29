import { waitForAsync, ComponentFixture, TestBed } from '@angular/core/testing';

import { SysLogComponent } from './log.component';

describe('SysLogComponent', () => {
  let component: SysLogComponent;
  let fixture: ComponentFixture<SysLogComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [SysLogComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SysLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { MedicalInfoService } from './medical-info.service';

describe('MedicalInfoServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MedicalInfoService = TestBed.get(MedicalInfoService);
    expect(service).toBeTruthy();
  });
});

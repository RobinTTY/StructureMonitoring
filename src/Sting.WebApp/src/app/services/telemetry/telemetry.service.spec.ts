import { TestBed } from '@angular/core/testing';

import { TelemetryService } from './telemetry.service';

describe('TelemetryDataImportService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TelemetryService = TestBed.get(TelemetryService);
    expect(service).toBeTruthy();
  });
});

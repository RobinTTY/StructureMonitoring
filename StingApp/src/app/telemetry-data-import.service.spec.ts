import { TestBed } from '@angular/core/testing';

import { TelemetryDataImportService } from './telemetry-data-import.service';

describe('TelemetryDataImportService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TelemetryDataImportService = TestBed.get(TelemetryDataImportService);
    expect(service).toBeTruthy();
  });
});

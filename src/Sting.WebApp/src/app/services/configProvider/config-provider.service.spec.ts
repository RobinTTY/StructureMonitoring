import { TestBed } from '@angular/core/testing';

import { ConfigProviderService } from './config-provider.service';

describe('ConfigProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ConfigProviderService = TestBed.get(ConfigProviderService);
    expect(service).toBeTruthy();
  });
});

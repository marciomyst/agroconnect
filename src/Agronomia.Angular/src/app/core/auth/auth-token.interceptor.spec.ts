import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ActiveOrganization } from '../organization/organization.model';
import { ActiveOrganizationStore } from '../organization/active-organization.store';
import { AuthTokenService } from './auth-token.service';
import { authTokenInterceptor } from './auth-token.interceptor';

describe('authTokenInterceptor', () => {
  let http: HttpClient;
  let httpMock: HttpTestingController;
  let token: string | null;
  let activeOrganization: ActiveOrganization | null;

  beforeEach(() => {
    token = 'test-token';
    activeOrganization = {
      organizationId: 'org-123',
      organizationName: 'Farm Alpha',
      organizationType: 'Farm',
      roles: ['Owner'],
    };

    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([authTokenInterceptor])),
        provideHttpClientTesting(),
        {
          provide: AuthTokenService,
          useValue: {
            getToken: () => token,
          },
        },
        {
          provide: ActiveOrganizationStore,
          useValue: {
            activeOrganization: () => activeOrganization,
          },
        },
      ],
    });

    http = TestBed.inject(HttpClient);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('adds organization header for authenticated API requests', () => {
    http.get('/api/orders').subscribe();

    const req = httpMock.expectOne('/api/orders');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    expect(req.request.headers.get('X-Organization-Id')).toBe('org-123');
    req.flush({});
  });

  it('does not add organization header when no active organization exists', () => {
    activeOrganization = null;

    http.get('/api/orders').subscribe();

    const req = httpMock.expectOne('/api/orders');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    expect(req.request.headers.has('X-Organization-Id')).toBeFalse();
    req.flush({});
  });
});

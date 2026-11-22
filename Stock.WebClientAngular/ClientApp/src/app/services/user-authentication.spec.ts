import { TestBed } from '@angular/core/testing';
import { UserAuthentication } from './user-authentication';

const STORAGE_KEY = 'oidc.user:http://localhost:44702:client_id_angular';

function storeUser(profile: Record<string, unknown>, expiresInSeconds: number): void {
  localStorage.setItem(
    STORAGE_KEY,
    JSON.stringify({
      access_token: 'token',
      token_type: 'Bearer',
      scope: 'openid profile StockAPI',
      profile: { sub: '1', ...profile },
      expires_at: Math.floor(Date.now() / 1000) + expiresInSeconds,
    }),
  );
}

describe('UserAuthentication', () => {
  let service: UserAuthentication;

  beforeEach(() => {
    localStorage.clear();
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserAuthentication);
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should load without a user when nobody is signed in', async () => {
    await expect(service.loadUser()).resolves.toBeUndefined();

    expect(service.isSignedIn()).toBe(false);
    expect(service.user()).toBeNull();
    await expect(service.getAccessToken()).resolves.toBeNull();
  });

  it('should restore the signed-in user from storage', async () => {
    storeUser({ name: 'Alice Smith', preferred_username: 'alice' }, 3600);

    await service.loadUser();

    expect(service.isSignedIn()).toBe(true);
    expect(service.user()).toEqual({ username: 'Alice Smith' });
    await expect(service.getAccessToken()).resolves.toBe('token');
  });

  it('should fall back to preferred_username and then to an empty name', async () => {
    storeUser({ preferred_username: 'alice' }, 3600);
    await service.loadUser();
    expect(service.user()).toEqual({ username: 'alice' });

    storeUser({}, 3600);
    await service.loadUser();
    expect(service.user()).toEqual({ username: '' });
  });

  it('should treat a user with an expired access token as signed out', async () => {
    storeUser({ name: 'Alice Smith' }, -60);

    await service.loadUser();

    expect(service.isSignedIn()).toBe(false);
    await expect(service.getAccessToken()).resolves.toBeNull();
  });
});

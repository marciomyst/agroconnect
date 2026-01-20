export type OrganizationType = 'Seller' | 'Farm';

export interface CurrentUserOrganization {
  organizationId: string;
  organizationName: string;
  type: OrganizationType;
  roles: string[];
}

export interface CurrentUserContext {
  userId: string;
  email: string;
  name?: string | null;
  organizations: CurrentUserOrganization[];
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  userId: string;
  email: string;
  accessToken?: string;
  token?: string;
  expiresAtUtc?: string;
}

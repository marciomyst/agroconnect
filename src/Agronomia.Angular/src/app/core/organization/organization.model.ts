import { OrganizationType } from '../auth/auth.types';

export interface ActiveOrganization {
  organizationId: string;
  organizationName: string;
  organizationType: OrganizationType;
  roles: string[];
}

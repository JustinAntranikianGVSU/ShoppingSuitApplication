import { User } from "./user"

export class ProfileData {
  isImpersonating: boolean
  loggedInUserProfile: User
  impersonationUserProfile: User
}

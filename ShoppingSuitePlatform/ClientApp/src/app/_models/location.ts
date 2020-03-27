import { UserBasic } from "./user"
import { AccessListBasic } from "./accessList"

export class LocationBasic {
  id: number
  name: string
}

export class Location extends LocationBasic {
  users: UserBasic[]
  accessLists: AccessListBasic[]
}
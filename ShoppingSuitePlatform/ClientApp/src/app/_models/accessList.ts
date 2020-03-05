import { User } from "./user"
import { Location } from "./location"

export class AccessListBasic {
  id: number
  name: string
}

export class AccessList extends AccessListBasic {
  locations: Location[]
  users: User[]
}
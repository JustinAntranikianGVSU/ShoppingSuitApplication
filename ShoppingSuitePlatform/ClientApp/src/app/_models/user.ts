export class User {
  id: number
  firstName: string
  lastName: string
  fullName: string
  email: string
  roles: Role[]
}

export class Role {
  id: number
  name: string
}
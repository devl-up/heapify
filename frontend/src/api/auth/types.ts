export interface UserDto {
  readonly id: string;
  readonly name: string;
}

export interface RegisterDto {
  readonly username: string;
  readonly email: string;
  readonly password: string;
}

export interface LoginDto {
  readonly email: string;
  readonly password: string;
}

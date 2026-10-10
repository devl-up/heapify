export interface UserDto {
  readonly name: string | null;
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

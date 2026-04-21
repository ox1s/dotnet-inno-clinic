import type { AuthTokens } from "@/lib/auth-util"

const BASE_URL = import.meta.env.VITE_API_BASE_URL

const LOGIN_URL = `/identity/authentication/login`

export interface LoginCredentials {
  email: string
  password: string
}

export async function loginUser(
  credentials: LoginCredentials
): Promise<AuthTokens> {
  const response = await fetch(LOGIN_URL, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(credentials),
  })

  if (!response.ok) {
    let errorMessage = "Login failed. Check your credentials and try again."

    try {
      const errorBody = (await response.json()) as {
        title?: string
        detail?: string
      }
      errorMessage = errorBody.detail ?? errorBody.title ?? errorMessage
    } catch {}

    throw new Error(errorMessage)
  }

  return (await response.json()) as AuthTokens
}

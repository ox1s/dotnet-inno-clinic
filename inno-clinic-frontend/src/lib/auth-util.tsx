import { createContext, useContext, useState, type ReactNode } from "react"

const AUTH_STORAGE_KEY = "auth"

export interface AuthTokens {
  token: string
  refreshToken: string
}

interface AuthContextProps {
  auth: AuthTokens | null
  isAuthenticated: boolean
  loginUser: (tokens: AuthTokens) => void
  logoutUser: () => void
}

const AuthContext = createContext<AuthContextProps | undefined>(undefined)

function readStoredAuth(): AuthTokens | null {
  if (typeof window === "undefined") {
    return null
  }

  const rawValue = sessionStorage.getItem(AUTH_STORAGE_KEY)

  if (!rawValue) {
    return null
  }

  try {
    return JSON.parse(rawValue) as AuthTokens
  } catch {
    sessionStorage.removeItem(AUTH_STORAGE_KEY)
    return null
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [auth, setAuth] = useState<AuthTokens | null>(() => readStoredAuth())

  const loginUser = (tokens: AuthTokens) => {
    sessionStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(tokens))
    setAuth(tokens)
  }

  const logoutUser = () => {
    sessionStorage.removeItem(AUTH_STORAGE_KEY)
    setAuth(null)
  }

  return (
    <AuthContext.Provider
      value={{
        auth,
        isAuthenticated: Boolean(auth?.token),
        loginUser,
        logoutUser,
      }}
    >
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth(): AuthContextProps {
  const context = useContext(AuthContext)

  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider")
  }

  return context
}

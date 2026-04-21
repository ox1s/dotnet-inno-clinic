import { Button } from "@/components/ui/button"
import {
  Card,
  CardAction,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Link, useLocation, useNavigate } from "react-router-dom"
import { useAuth } from "@/lib/auth-util"
import { useState, type FormEvent } from "react"
import { loginUser } from "@/services/login"

export default function SignInPage() {
  const navigate = useNavigate()
  const location = useLocation()
  const { loginUser: saveAuth } = useAuth()
  const [email, setEmail] = useState("")
  const [password, setPassword] = useState("")
  const [errorMessage, setErrorMessage] = useState("")
  const [isSubmitting, setIsSubmitting] = useState(false)

  const redirectPath =
    (location.state as { from?: { pathname?: string } } | null)?.from
      ?.pathname ?? "/"

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault()

    try {
      setIsSubmitting(true)
      setErrorMessage("")
      const auth = await loginUser({ email, password })
      saveAuth(auth)
      navigate(redirectPath, { replace: true })
    } catch (error) {
      setErrorMessage(
        error instanceof Error ? error.message : "Unexpected login error."
      )
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <Card className="mx-auto mt-20 w-full max-w-sm">
      <CardHeader>
        <CardTitle>Login to your account</CardTitle>
        <CardDescription>
          Enter your email below to login to your account
        </CardDescription>
        <CardAction>
          <Button variant="link" asChild>
            <Link to="/signup">Sign Up</Link>
          </Button>
        </CardAction>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit}>
          <div className="flex flex-col gap-6">
            <div className="grid gap-2">
              <Label htmlFor="email">Email</Label>
              <Input
                id="email"
                type="email"
                placeholder="m@example.com"
                required
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>
            <div className="grid gap-2">
              <div className="flex items-center">
                <Label htmlFor="password">Password</Label>
                <a
                  href="#"
                  className="ml-auto inline-block text-sm underline-offset-4 hover:underline"
                >
                  Forgot your password?
                </a>
              </div>
              <Input
                id="password"
                type="password"
                required
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>
            {errorMessage ? (
              <p className="text-sm text-destructive">{errorMessage}</p>
            ) : null}
            <Button type="submit" className="w-full" disabled={isSubmitting}>
              {isSubmitting ? "Logging in..." : "Login"}
            </Button>
            <Button variant="outline" className="w-full" type="button">
              Login with Google
            </Button>
          </div>
        </form>
      </CardContent>
    </Card>
  )
}

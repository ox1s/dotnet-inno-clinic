import { ThemeProvider } from "@/components/theme-provider"
import { Navbar } from "@/components/Navbar"
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom"
import SignInPage from "./pages/Auth/SignInPage"
import SignUpPage from "./pages/Auth/SignUpPage"
import Home from "./pages/Home"

function App() {
  return (
    <ThemeProvider defaultTheme="light" storageKey="vite-ui-theme">
      <BrowserRouter>
        <div className="min-h-screen bg-background font-sans text-foreground selection:bg-primary/20">
          <Navbar />
          <main className="mx-auto min-h-screen max-w-6xl px-5 pt-24 md:px-10 md:pt-28">
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/signin" element={<SignInPage />} />
              <Route path="/signup" element={<SignUpPage />} />
              <Route path="*" element={<Navigate to="/" replace />} />
            </Routes>
          </main>
        </div>
      </BrowserRouter>
    </ThemeProvider>
  )
}

export default App

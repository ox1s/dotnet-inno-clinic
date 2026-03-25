import { ThemeProvider } from "@/components/theme-provider"
import { Navbar } from "@/components/Navbar"
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom"
import SignInPage from "./pages/Auth/SignInPage"
import SignUpPage from "./pages/Auth/SignUpPage"
import Home from "./pages/Home"

function App() {
  return (
    <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
      <BrowserRouter>
        <div className="min-h-screen bg-background text-foreground font-sans selection:bg-primary/20">
          <Navbar />
          <main className="pt-32 px-6 md:px-12 max-w-7xl mx-auto min-h-screen">
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
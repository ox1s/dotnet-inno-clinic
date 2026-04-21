import { Button } from "@/components/ui/button"
import {
  StethoscopeIcon,
  FirstAidIcon,
  CalendarCheckIcon,
  ArrowRightIcon,
} from "@phosphor-icons/react"

export default function Home() {
  return (
    <div className="flex flex-col gap-24 pb-24">
      {/* Hero Section */}
      <section className="relative flex flex-col items-start gap-8 py-16 text-left">
        <div className="inline-flex animate-in items-center gap-2 bg-primary/10 px-4 py-2 text-sm font-semibold text-primary duration-1000 fade-in slide-in-from-top-4">
          <StethoscopeIcon size={20} weight="bold" />
          <span>New: Advanced Diagnostic Center open now</span>
        </div>
        <h1 className="max-w-4xl bg-linear-to-b from-foreground to-foreground/70 bg-clip-text text-5xl font-bold tracking-tight text-transparent md:text-7xl">
          Comprehensive Care for Your{" "}
          <span className="text-primary">Whole Family</span>
        </h1>
        <p className="max-w-2xl text-xl leading-relaxed text-pretty text-muted-foreground">
          Welcome to InnoClinic, where cutting-edge technology meets
          compassionate care. Manage your health journey with our innovative
          clinic platform.
        </p>
        <div className="mt-4 flex flex-wrap items-center justify-start gap-4">
          <Button size="lg" className="group h-14 gap-2 px-8 text-lg">
            Book Appointment
            <CalendarCheckIcon
              size={24}
              className="transition-transform group-hover:translate-x-1"
            />
          </Button>
          <Button variant="outline" size="lg" className="h-14 px-8 text-lg">
            Our Services
          </Button>
        </div>
      </section>

      {/* Quick Services */}
      <section className="grid grid-cols-1 gap-8 md:grid-cols-3">
        {[
          {
            title: "Expert Doctors",
            desc: "Highly qualified specialists in various medical fields.",
            icon: StethoscopeIcon,
          },
          {
            title: "Primary Care",
            desc: "Personalized healthcare services for you and your family.",
            icon: FirstAidIcon,
          },
          {
            title: "Easy Scheduling",
            desc: "Book and manage your appointments online in seconds.",
            icon: CalendarCheckIcon,
          },
        ].map((item, i) => (
          <div
            key={i}
            className="group border border-border bg-card/50 p-8 transition-all duration-300 hover:bg-card hover:shadow-2xl hover:shadow-primary/5"
          >
            <div
              className={`mb-6 flex h-14 w-14 items-center justify-center transition-transform group-hover:scale-110`}
            >
              <item.icon size={32} weight="fill" />
            </div>
            <h3 className="mb-3 text-2xl font-bold">{item.title}</h3>
            <p className="mb-6 leading-relaxed text-muted-foreground">
              {item.desc}
            </p>
            <Button
              variant="link"
              className="group/btn h-auto gap-2 p-0 font-semibold"
            >
              Learn more
              <ArrowRightIcon
                size={18}
                className="transition-transform group-hover/btn:translate-x-1"
              />
            </Button>
          </div>
        ))}
      </section>
    </div>
  )
}

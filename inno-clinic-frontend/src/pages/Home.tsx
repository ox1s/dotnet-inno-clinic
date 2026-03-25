import { Button } from "@/components/ui/button";
import { Link } from "react-router-dom";
import {
  StethoscopeIcon,
  FirstAidIcon,
  CalendarCheckIcon,
  ArrowRightIcon
} from "@phosphor-icons/react";

export default function Home() {
  return (
    <div className="flex flex-col gap-24 pb-24">
      {/* Hero Section */}
      <section className="relative flex flex-col items-center text-center gap-8 py-16">
        <div className="inline-flex items-center gap-2 px-4 py-2 bg-primary/10 text-primary text-sm font-semibold animate-in fade-in slide-in-from-top-4 duration-1000">
          <StethoscopeIcon size={20} weight="bold" />
          <span>New: Advanced Diagnostic Center open now</span>
        </div>

        <h1 className="text-5xl md:text-7xl font-bold tracking-tight max-w-4xl bg-gradient-to-b from-foreground to-foreground/70 bg-clip-text text-transparent">
          Comprehensive Care for Your <span className="text-primary">Whole Family</span>
        </h1>

        <p className="text-xl text-muted-foreground max-w-2xl text-pretty leading-relaxed">
          Welcome to InnoClinic, where cutting-edge technology meets compassionate care.
          Manage your health journey with our innovative clinic platform.
        </p>

        <div className="flex flex-wrap items-center justify-center gap-4 mt-4">
          <Button size="lg" className="h-14 px-8 gap-2 group text-lg">
            Book Appointment
            <CalendarCheckIcon size={24} className="group-hover:translate-x-1 transition-transform" />
          </Button>
          <Button variant="outline" size="lg" className="h-14 px-8 text-lg">
            Our Services
          </Button>
        </div>
      </section>

      {/* Quick Services */}
      <section className="grid grid-cols-1 md:grid-cols-3 gap-8">
        {[
          {
            title: "Expert Doctors",
            desc: "Highly qualified specialists in various medical fields.",
            icon: StethoscopeIcon
          },
          {
            title: "Primary Care",
            desc: "Personalized healthcare services for you and your family.",
            icon: FirstAidIcon
          },
          {
            title: "Easy Scheduling",
            desc: "Book and manage your appointments online in seconds.",
            icon: CalendarCheckIcon
          }
        ].map((item, i) => (
          <div
            key={i}
            className="group p-8 border border-border bg-card/50 hover:bg-card hover:shadow-2xl hover:shadow-primary/5 transition-all duration-300"
          >
            <div className={`w-14 h-14 flex items-center justify-center mb-6 group-hover:scale-110 transition-transform `}>
              <item.icon size={32} weight="fill" />
            </div>
            <h3 className="text-2xl font-bold mb-3">{item.title}</h3>
            <p className="text-muted-foreground leading-relaxed mb-6">
              {item.desc}
            </p>
            <Button variant="link" className="p-0 h-auto gap-2 group/btn font-semibold">
              Learn more
              <ArrowRightIcon size={18} className="group-hover/btn:translate-x-1 transition-transform" />
            </Button>
          </div>
        ))}
      </section>
    </div>
  );
}

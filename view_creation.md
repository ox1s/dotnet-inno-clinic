# appointment.appointments_view

**This view is now created by an EF migration — do not apply it by hand.**

See `inno-clinic-backend/InnoClinic.Appointment/src/Appointment.Api/Data/Migrations/*_ResyncAppointmentsView.cs`,
which is the single source of truth for the definition. It runs automatically on
`appointment-api` startup along with every other migration.

History: the original `AddAppointmentsView` migration created a view that was missing
`patient_middle_name`, `office_id` and `office_address`, while `AppointmentDbContext` mapped
all three — so every query against `AppointmentViews` failed with `42703 column does not
exist`. The two follow-up migrations intended to fix it shipped with empty `Up()` bodies, and
this file held the hand-applied correction. `ResyncAppointmentsView` folds that correction
back into the migration history.

The view spans four schemas (`appointment`, `profile`, `identity`, `clinic_management`), so
`appointment-api` must start after the services that own them — the AppHost already enforces
this with `WaitFor`.

## Known limitation

`local_date` is `CAST(duration_start AS DATE)`, i.e. the date in **UTC**, while
`ListReceptionistAppointmentsHandler` filters by the clinic-local date from
`Appointments_Resourses.Clinics_TimeZone`. Appointments close to midnight can therefore land
on the adjacent `local_date`. Fixing it means deciding where the clinic timezone lives (a
literal in the view, a `SET TIME ZONE` on the connection, or dropping `local_date` and
filtering on `duration_start` ranges instead).

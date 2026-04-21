```sql
CREATE OR REPLACE VIEW appointment.appointments_view AS
SELECT
a.appointment_id,
CAST(a.duration_start AS DATE) AS local_date,
a.duration_start AS duration_start,
a.duration_end AS duration_end,
a.is_approved,

        a.doctor_id,
        d.first_name AS doctor_first_name,
        d.last_name AS doctor_last_name,
        d.middle_name AS doctor_middle_name,

        a.patient_id,
        p.first_name AS patient_first_name,
        p.last_name AS patient_last_name,
        p.middle_name AS patient_middle_name,

        acc.phone_number AS patient_phone,

        a.service_id,
        s."ServiceName" AS service_name,

        a.office_id,
        o."Address" AS office_address

    FROM appointment.appointments a
    LEFT JOIN profile.doctors d ON a.doctor_id = d.account_profile_id
    LEFT JOIN profile.patients p ON a.patient_id = p.account_profile_id

    LEFT JOIN identity.accounts acc ON p.account_id = acc."Id"
    LEFT JOIN clinic_management."Services" s ON a.service_id = s."Id"
    LEFT JOIN clinic_management."Offices" o ON a.office_id = o."Id";
```

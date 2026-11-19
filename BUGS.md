# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| R5 (rezervare în trecut + TimeProvider) | src/RoomHub.Api/Services/BookingService.cs, 8-45 | AI în timp ce scria | Să ia timpul curent UTC din TimeProvider și să respingă cu 400 Bad Request orice rezervare cu startUtc < nowUtc. |
| R6 (durată maximă 4 ore) | src/RoomHub.Api/Services/BookingService.cs, 17-18 | AI în timp ce scria | Să respingă cu 400 Bad Request orice rezervare cu durata strict mai mare de 4 ore, permițând exact 4 ore. |
| R7 (intervale adiacente) | src/RoomHub.Api/Services/BookingService.cs, 28 | AI în timp ce scria | Să verifice suprapunerea pe intervale cu < în loc de <=, astfel încât intervalele care doar se ating să fie permise. |
| R7 + R8 (rezervările anulate trebuie să elibereze intervalul) | src/RoomHub.Api/Services/BookingService.cs, 26–30 și Repositories/BookingRepository.cs, 27–33 | AI în timp ce scria | Să verifice dacă suprapunerile sunt BookingStatus.Active, astfel încât o rezervare anulată să elibereze intervalul. |
| R8 (anulare dublă) | src/RoomHub.Api/Services/BookingService.cs, 56–60 | AI în timp ce scria | Dacă rezervarea a fost deja anulată, returnează 409 Conflict. |
| Contract API (date normalizate în UTC & enum-uri JSON) | src/RoomHub.Api/Services/BookingService.cs, 17–39 și src/RoomHub.Api/Program.cs, 6–14 | AI în timp ce scria | Să normalizeze captele la UTC prin DateTimeKind.Utc, iar Program.cs să înregistreze JsonStringEnumConverter în opțiunile JSON HTTP. |
# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?

R3, R4 și R7 (mai exact partea de Booking). Era în Services/BookingService.cs, iar acum se află în Domain/Booking.cs. Se află acolo din cauza conceptului de Rich Domain, unde entitățile sunt create de către un static factory method care face și verificările automat și nu permite să creezi entități invalide.

## 2. Result instead of an exception
Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?

Deoarece, de exemplu, dacă o cameră ar fi ocupată, acesta nu ar fi o eroare tehnică, ci un caz normal al business-ului. Result<T> face eșecul vizibil direct din variabilele ei.
Când un Error este returnat, se returnează cu ErrorType.<tipul problemei>, iar endpoint-ul apelează Error.ToProblem() pentru a-l transforma într-un răspuns HTTP corespunzător.

## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

Doar clasele din Infrastructure/Persistence, Program.cs (pentru inițializare) și endpoint-urile de citire știu de EF Core.
Câștigăm decuplarea domeniului și posibilitatea de a testa handlerele cu date fake, însă plătim prin codul suplimentar pe care trebuie să-l scriem.

## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

-

## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

După primul pas de refactorizare, AI-ul a modelat și operațiile de citire ca ICommand (GetRoomCommand și GetBookingCommand), pe care le-am respins și înlocuit cu GetRoomEndpoint.cs și GetBookingEndpoint.cs.

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?

Aș face unit tests pentru mai multe situații. Nu văd nici un risc rămas.
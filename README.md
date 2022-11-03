# TennisCourt

This is a backend API to manage reservations for a Tennis Court. 
The API is created but it's missing the business requirements.

# Business Requirements

- ProcessReservation:
Creation of the actual Reservation with the attributes from Reservation Model.
  
- CancelReservation:
Cancellation of the Reservation, using the RefundValue.
  
- RescheduleReservation:
Reschedule the reservation to a new date when there's no schedule set.
  
- GetReservation:
Gets the Reservation details.

# Technical requirements

- The API features listed above are part of the incomplete service ReservationAppService, complete the service with the missing implementation.

- For each of these functionalities it also needs to have an endpoint on the ReservationController.

- We use Autommapper on the solution, you can map your DTO to Entitites using it.

- EntityFrameworkCore is used on the API, there's a configuration file for it already, create the migration for the solution.

- Create Unit Tests for the service layer.

- Create other domain models if needed, if you have any suggestion to the solution, feel free to add it.


Good luck!! :)

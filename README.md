# FlightChangeBusinessOpportunity
Implement an algorithm to detect flight schedule changes in airline competitive landscape.

Description

You are creating an application, to be used by airline agencies, for detecting cases of airlines introducing
new or discontinuing existing regular flight schedules.
For example, AirAsia is offering a flight from KUL to Delhi every Monday at 7:30. In the case that
AirAsia decides to stop offering that flight, we need to be able to detect that, because that opens a
business opportunity for flight agencies to offer alternative flights. The same applies to newly introduced
flight schedules.

Change detection algorithm

New flights: A flight with departure time T is considered to be a new flight if no corresponding
flight from same airline exists with departure time T’ = T - 7 days (+/- 30 minutes
tolerance).
Discontinued flights: A flight with departure time T is considered to be discontinued if no
corresponding flight from same airline exists with departure time T’ = T + 7 days
(+/- 30 minutes tolerance).

Terminology

Route 
Unique combination of origin, destination and departure date.
Flight Concrete flight scheduled per airline. Flights are in many-to-one relationship with
routes.

Subscription 
Origin/destination pair an agency is interested in.

Input
You will be provided with the sample data set consisting of three CSV files with following structure:

routes.csv 
- route_id
- origin_city_id
- destination_city_id
- departure_date

flights.csv 
- flight_id
- route_id1
- departure_time2
- arrival_time2
- airline_id

subscriptions.csv
- agency_id
- origin_city_id
- destination_city_id
1
 Foreign key to route table.
2
 Includes both date and time.
The data set intentionally omits city, airline and agency tables, because they’re not crucial for the
implementation of the algorithm.

Solution 

1. Create a database with table structure to accommodate provided data and import the data into
database tables as it is (no transformation needed – foreign keys already match).
2. Implement a data access layer (preferably using Entity Framework).
3. Implement a .NET console application taking three command line parameters:
- start date (in yyyy-mm-dd format)
- end date (in yyyy-mm-dd format)
- agency id (used to filter the flights an agency is interested in, by applying subscriptions)
Example call: YourProgram.exe 2018-01-01 2018-01-15 1

4. Output the results in below Column, containing following columns:
- flight_id
- origin_city_id
- destination_city_id
- departure_time
- arrival_time
- airline_id
- status (New / Discontinued)

alter procedure getReasultUsingChangeDetectionAlgo
   @startdt	datetime,	-- startdt 
    @enddt	datetime,	-- enddt
	@agencyid int	-- Agency ID
AS
begin


IF OBJECT_ID(N'dbo.temp_f', N'U') IS NOT NULL  
   DROP TABLE [dbo].temp_f;  

   IF OBJECT_ID(N'dbo.temp_ontime', N'U') IS NOT NULL  
   DROP TABLE [dbo].temp_ontime;  




--select * from temp_f
select   f1.* into temp_f
from  [Temp-f1] f1
where 
f1.departure_time 
between dateadd(day, -7,dateadd(minute, -30,@startdt)) and 
dateadd(day, +8,dateadd(minute, +30,@enddt))
 

  select   f1.route_id, f1.flight_id, r.origin_city_id ,r.destination_city_id ,
f1.departure_time , f1.arrival_time ,f1.airline_id , r.departure_date,s.agency_id, 'ontime' status into  temp_ontime
from temp_f f1 , [Temp-r1] r 
, [dbo].[temp-s] s
where f1.route_id = r.route_id
and r.origin_city_id  =s.origin_city_id
and r.destination_city_id  =s.destination_city_id
and  f1.departure_time  between   @startdt and  @enddt
and s.agency_id =@agencyid 



select distinct f1.route_id, f1.flight_id, r.origin_city_id ,r.destination_city_id ,
f1.departure_time , f1.arrival_time ,f1.airline_id , r.departure_date,s.agency_id ,'New' status from  temp_f  f1,(select flight_id,
route_id, airline_id, origin_city_id ,destination_city_id,
dateadd(day, -7,dateadd(minute, -30,departure_time)) startdt,dateadd(day, -7,dateadd(minute, +30,departure_time)) enddt 
, departure_time, arrival_time
from temp_ontime
--where 
--flight_id = 1321941
) t , [Temp-r1] r , [dbo].[temp-s] s where 
f1.route_id  = r.route_id and 
t.origin_city_id  = r.origin_city_id and 
t.destination_city_id  = r.destination_city_id and 
r.origin_city_id  = s.origin_city_id and 
r.destination_city_id  = s.destination_city_id
and f1.departure_time between  t.startdt and t.enddt
and f1.airline_id <>  t.airline_id
and s.agency_id =@agencyid 

select distinct  f1.route_id, f1.flight_id, r.origin_city_id ,r.destination_city_id ,
f1.departure_time , f1.arrival_time ,f1.airline_id , r.departure_date,s.agency_id ,'Discontinued' status from  temp_f  f1,(select flight_id,
route_id, airline_id, origin_city_id ,destination_city_id,
dateadd(day, +7,dateadd(minute, -30,departure_time)) startdt,dateadd(day, +7,dateadd(minute, +30,departure_time)) enddt 
, departure_time, arrival_time
from temp_ontime
--where 
--flight_id = 1321941
) t , [Temp-r1] r , [dbo].[temp-s] s where 
f1.route_id  = r.route_id and 
t.origin_city_id  = r.origin_city_id and 
t.destination_city_id  = r.destination_city_id and 
r.origin_city_id  = s.origin_city_id and 
r.destination_city_id  = s.destination_city_id
and f1.departure_time between  t. startdt and t.enddt
and f1.airline_id <>  t.airline_id
and s.agency_id =@agencyid

end 
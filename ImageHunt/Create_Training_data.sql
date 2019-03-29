
insert into Games (IsDeleted, Name, StartDate, IsActive, MapCenterLat, MapCenterLng, MapZoom,
                   Description, PictureId)
select false, 'Test', g.StartDate, true, g.MapCenterLat, g.MapCenterLng, g.MapZoom, 'Test set', g.PictureId
from Games g
where g.Id = 29;


select @gameId := last_insert_id();

insert into Nodes (Discriminator, GameId, Latitude, Longitude, Name, IsDeleted, Question, Delay, ImageId, Action, Points, Password, LocationHint, Answer, Choice, BonusType, Location)
select n.Discriminator, @gameId, n.Latitude, n.Longitude, n.Name, n.IsDeleted , n.Question, n.delay, n.imageid, n.action, n.points, n.password, n.locationhint, n.answer, n.choice, n.bonustype,
       n.location
from Nodes n
where n.GameId = 29
and n.IsDeleted = false;

insert into Teams (IsDeleted, Name, GameId, Color, CultureInfo)
values (false, 'Team1', @gameId, '0xFF0000', 'en');
select @team1Id := last_insert_id();

insert into Teams (IsDeleted, Name, GameId, Color, CultureInfo)
values (false, 'Team2', @gameId, '0x00FF00', 'en');
select @team2Id := last_insert_id();

insert into GameActions (DateOccured, GameId, IsDeleted, Latitude, Longitude, PictureId, Action, IsValidated, TeamId)
select NOW(), @gameId, false, ga.Latitude, ga.Longitude, ga.PictureId, ga.Action, false, case when rand()<0.5 then @team1Id else @team2Id end
from GameActions ga
where ga.GameId = 29;

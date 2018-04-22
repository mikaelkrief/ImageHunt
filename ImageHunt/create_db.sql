CREATE SCHEMA ImageHunt;
create USER 'HuntUser'@'localhost'
  IDENTIFIED  by 'jhns_U4Has9'

GRANT ALL PRIVILEGES on ImageHunt.* to'HuntUser'@'localhost';
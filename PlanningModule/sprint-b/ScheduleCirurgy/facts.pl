% Exemplo de agendas de médicos e horários de trabalho
agenda_staff(d001,20241028,[(720,790,m01),(1080,1140,c01)]).
agenda_staff(d002,20241028,[(850,900,m02),(901,960,m02),(1380,1440,c02)]).
agenda_staff(d003,20241028,[(720,790,m01),(910,980,m02)]).

timetable(d001,20241028,(480,1200)).
timetable(d002,20241028,(500,1440)).
timetable(d003,20241028,(520,1320)).

% Definições de médicos e cirurgias com tempos de anestesia, cirurgia e limpeza
staff(d001,doctor,orthopaedist,[so2,so3,so4]).
staff(d002,doctor,orthopaedist,[so2,so3,so4]).
staff(d003,doctor,orthopaedist,[so2,so3,so4]).

staff(d004,doctor,anesthesia,[so2,so3,so4]).  % Médico anestesista

staff(n001,nurse,anesthesia,[so2,so3,so4]).  % Enfermeiro instrumentador
staff(n002,nurse,cleaning,[so2,so3,so4]).   % Enfermeiro circulante
staff(n003,nurse,anesthesia,[so2,so3,so4]).    % Enfermeiro anestesista
staff(n004,nurse,cleaning,[so2,so3,so4]).      % Enfermeiro de limpeza

% surgery(SurgeryType, TAnesthesia, TSurgery, TCleaning).
surgery(so2, 45, 60, 45).  % 45min de anestesia, 60min de cirurgia, 45min de limpeza
surgery(so3, 45, 90, 45).  % 45min de anestesia, 90min de cirurgia, 45min de limpeza
surgery(so4, 45, 75, 45).  % 45min de anestesia, 75min de cirurgia, 45min de limpeza

surgery_id(so100001,so2).
surgery_id(so100002,so3).
surgery_id(so100003,so4).
surgery_id(so100004,so2).
surgery_id(so100005,so4).


assignment_surgery(so100001,d001).

assignment_surgery(so100002,d002).
assignment_surgery(so100003,d003).
assignment_surgery(so100004,d001).
assignment_surgery(so100004,d002).
assignment_surgery(so100005,d002).
assignment_surgery(so100005,d003).

% Agenda inicial da sala de operações
agenda_operation_room(or1,20241028,[(520,579,so100000),(1000,1059,so099999)]).

% Agendas dos novos membros da equipe
agenda_staff(d004, 20241028, []).  % Anestesista d004 sem agenda específica
agenda_staff(n001, 20241028, []).  % Enfermeiro instrumentador n001 sem agenda
agenda_staff(n002, 20241028, []).  % Enfermeiro circulante n002 sem agenda
agenda_staff(n003, 20241028, []).  % Enfermeiro anestesista n003 sem agenda
agenda_staff(n004, 20241028, []).  % Enfermeiro de limpeza n004 sem agenda

% Horários de trabalho (Timetable) para os novos membros
timetable(d004,20241028,(480,1440)).  % Anestesista d004
timetable(n001,20241028,(480,1440)).  % Enfermeiro instrumentador n001
timetable(n002,20241028,(480,1440)).  % Enfermeiro circulante n002
timetable(n003,20241028,(480,1440)).  % Enfermeiro anestesista n003
timetable(n004,20241028,(480,1440)).  % Enfermeiro de limpeza n004
